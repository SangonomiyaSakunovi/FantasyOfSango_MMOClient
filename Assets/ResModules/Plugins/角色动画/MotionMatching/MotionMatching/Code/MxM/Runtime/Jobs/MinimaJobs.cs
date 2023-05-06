// ================================================================================================
// File: MinimaJobs.cs
// 
// Authors:  Kenneth Claassen
// Date:     2019-5-03: Created this file.
// 
//     Contains a part of the 'MxM' namespace for 'Unity Engine'.
// ================================================================================================
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace MxM
{
    //============================================================================================
    /**
    *  @brief Job struct used to find the minima of costs from a list of pose and trajectory 
    *  costs. The minima job is also responsible for applying pose favouring of any kind to the 
    *  costs before comparing.
    *  
    *  This is the most basic FindMinima job 
    *         
    *********************************************************************************************/
    [BurstCompile(CompileSynchronously = true)]
    public struct FindMinima : IJobParallelForBatch
    {
        [ReadOnly]
        public NativeArray<float> PoseCosts;

        [ReadOnly]
        public NativeArray<float> TrajCosts;

        [ReadOnly]
        public NativeArray<float> PoseFavour;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<int> ChosenPoseId;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<float> ChosenPoseCost;

        [ReadOnly]
        public int BatchSize;

        public void Execute(int startIndex, int count)
        {
            float bestCost = float.MaxValue;
            int bestPoseId = 0;

            for (int i = 0; i < count; ++i)
            {
                int index = startIndex + i;
                float goalCost = (PoseCosts[index] + TrajCosts[index]) * PoseFavour[index];

                if (goalCost < bestCost)
                {
                    bestCost = goalCost;
                    bestPoseId = index;
                }
            }

            int batchId = startIndex / BatchSize;

            ChosenPoseId[batchId] = bestPoseId;
            ChosenPoseCost[batchId] = bestCost;
        }
    }//End of struct: FindMinima

    //============================================================================================
    /**
    *  @brief Job struct used to find the minima of costs from a list of pose and trajectory 
    *  costs. The minima job is also responsible for applying pose favouring of any kind to the 
    *  costs before comparing.
    *  
    *  This find minima job has added functionality to ensure that the current clip is not 
    *  chosen again.
    *         
    *********************************************************************************************/
    [BurstCompile(CompileSynchronously = true)]
    public struct FindMinima_EnforceClipChange : IJobParallelForBatch
    {
        //Output
        [ReadOnly]
        public NativeArray<float> PoseCosts;

        [ReadOnly]
        public NativeArray<float> TrajCosts;

        [ReadOnly]
        public NativeArray<float> PoseFavour;

        [ReadOnly]
        public NativeArray<int> PoseClipIds;

        [ReadOnly]
        public int CurrentClipId;

        [ReadOnly]
        public int BatchSize;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<int> ChosenPoseId;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<float> ChosenPoseCost;

        public void Execute(int startIndex, int count)
        {
            float bestCost = float.MaxValue;
            int bestPoseId = 0;

            for (int i = 0; i < count; ++i)
            {
                int index = startIndex + i;

                if (CurrentClipId == PoseClipIds[index])
                    continue;

                float goalCost = (PoseCosts[index] + TrajCosts[index]) * PoseFavour[index];

                if (goalCost < bestCost)
                {
                    bestCost = goalCost;
                    bestPoseId = index;
                }
            }


            int batchId = startIndex / BatchSize;

            ChosenPoseId[batchId] = bestPoseId;
            ChosenPoseCost[batchId] = bestCost;
        }
    }//End of struct: FindMinima_EnforceClipChange

    //============================================================================================
    /**
    *  @brief Job struct used to find the minima of costs from a list of pose and trajectory 
    *  costs. The minima job is also responsible for applying pose favouring of any kind to the 
    *  costs before comparing.
    *  
    *  This FindMinima job also calculates favour tags into cost comparrisons. it is only used
    *  by the MxMAnimator when there are favoured tags.
    *         
    *********************************************************************************************/
    [BurstCompile(CompileSynchronously = true)]
    public struct FindMinima_FavourExclusive : IJobParallelForBatch
    {
        //Input
        [ReadOnly]
        public NativeArray<float> PoseCosts;

        [ReadOnly]
        public NativeArray<float> TrajCosts;

        [ReadOnly]
        public NativeArray<float> PoseFavour;

        [ReadOnly]
        public NativeArray<ETags> PoseFavourTags;

        [ReadOnly]
        public int BatchSize;

        //Output
        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<int> ChosenPoseId;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<float> ChosenPoseCost;

        [ReadOnly]
        public ETags FavourTags;

        [ReadOnly]
        public float FavourMultiplier;

        public void Execute(int startIndex, int count)
        {
            float bestCost = float.MaxValue;
            int bestPoseId = 0;

            for (int i = 0; i < count; ++i)
            {
                int index = startIndex + i;

                float curCost = (PoseCosts[index] + TrajCosts[index]) * PoseFavour[index];

                if ((PoseFavourTags[index] & FavourTags) == FavourTags)
                    curCost *= FavourMultiplier;

                if (curCost < bestCost)
                {
                    bestCost = curCost;
                    bestPoseId = index;
                }
            }

            int batchId = startIndex / BatchSize;

            ChosenPoseId[batchId] = bestPoseId;
            ChosenPoseCost[batchId] = bestCost;
        }
    }//End of struct: FindMinima_Favour

    //============================================================================================
    /**
    *  @brief Job struct used to find the minima of costs from a list of pose and trajectory 
    *  costs. The minima job is also responsible for applying pose favouring of any kind to the 
    *  costs before comparing.
    *  
    *  This is the most complex FindMinima job and it contains all functionality from the base
    *  find Minima job while also applying favour tag multipliers and enforcing clip changes.
    *         
    *********************************************************************************************/
    [BurstCompile(CompileSynchronously = true)]
    public struct FindMinima_FavourExclusive_EnforceClipChange : IJobParallelForBatch
    {
        //Input
        [ReadOnly]
        public NativeArray<float> PoseCosts;

        [ReadOnly]
        public NativeArray<float> TrajCosts;

        [ReadOnly]
        public NativeArray<float> PoseFavour;

        [ReadOnly]
        public NativeArray<ETags> PoseFavourTags;

        [ReadOnly]
        public int BatchSize;

        [ReadOnly]
        public NativeArray<int> PoseClipIds;

        [ReadOnly]
        public int CurrentClipId;

        [ReadOnly]
        public ETags FavourTags;

        [ReadOnly]
        public float FavourMultiplier;

        //Output
        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<int> ChosenPoseId;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<float> ChosenPoseCost;

        public void Execute(int startIndex, int count)
        {
            float bestCost = float.MaxValue;
            int bestPoseId = 0;

            for (int i = 0; i < count; ++i)
            {
                int index = startIndex + i;

                if (CurrentClipId == PoseClipIds[index])
                    continue;

                float curCost = (PoseCosts[index] + TrajCosts[index]) * PoseFavour[index];

                if ((PoseFavourTags[index] & FavourTags) == FavourTags)
                    curCost *= FavourMultiplier;

                if (curCost < bestCost)
                {
                    bestCost = curCost;
                    bestPoseId = index;
                }
            }

            int batchId = startIndex / BatchSize;

            ChosenPoseId[batchId] = bestPoseId;
            ChosenPoseCost[batchId] = bestCost;
        }
    }//End of struct: FindMinima_Favour_EnforceClipChange

    //============================================================================================
    /**
    *  @brief Job struct used to find the minima of costs from a list of pose and trajectory 
    *  costs. The minima job is also responsible for applying pose favouring of any kind to the 
    *  costs before comparing.
    *  
    *  This FindMinima job also calculates favour tags into cost comparrisons. it is only used
    *  by the MxMAnimator when there are favoured tags.
    *         
    *********************************************************************************************/
    [BurstCompile(CompileSynchronously = true)]
    public struct FindMinima_FavourInclusive : IJobParallelForBatch
    {
        //Input
        [ReadOnly]
        public NativeArray<float> PoseCosts;

        [ReadOnly]
        public NativeArray<float> TrajCosts;

        [ReadOnly]
        public NativeArray<float> PoseFavour;

        [ReadOnly]
        public NativeArray<ETags> PoseFavourTags;

        [ReadOnly]
        public int BatchSize;

        //Output
        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<int> ChosenPoseId;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<float> ChosenPoseCost;

        [ReadOnly]
        public ETags FavourTags;

        [ReadOnly]
        public float FavourMultiplier;

        public void Execute(int startIndex, int count)
        {
            float bestCost = float.MaxValue;
            int bestPoseId = 0;

            for (int i = 0; i < count; ++i)
            {
                int index = startIndex + i;

                float curCost = (PoseCosts[index] + TrajCosts[index]) * PoseFavour[index];

                if ((PoseFavourTags[index] & FavourTags) != 0)
                    curCost *= FavourMultiplier;

                if (curCost < bestCost)
                {
                    bestCost = curCost;
                    bestPoseId = index;
                }
            }

            int batchId = startIndex / BatchSize;

            ChosenPoseId[batchId] = bestPoseId;
            ChosenPoseCost[batchId] = bestCost;
        }
    }//End of struct: FindMinima_FavourInclusive

    //============================================================================================
    /**
    *  @brief Job struct used to find the minima of costs from a list of pose and trajectory 
    *  costs. The minima job is also responsible for applying pose favouring of any kind to the 
    *  costs before comparing.
    *  
    *  This is the most complex FindMinima job and it contains all functionality from the base
    *  find Minima job while also applying favour tag multipliers and enforcing clip changes.
    *         
    *********************************************************************************************/
    [BurstCompile(CompileSynchronously = true)]
    public struct FindMinima_FavourInclusive_EnforceClipChange : IJobParallelForBatch
    {
        //Input
        [ReadOnly]
        public NativeArray<float> PoseCosts;

        [ReadOnly]
        public NativeArray<float> TrajCosts;

        [ReadOnly]
        public NativeArray<float> PoseFavour;

        [ReadOnly]
        public NativeArray<ETags> PoseFavourTags;

        [ReadOnly]
        public int BatchSize;

        [ReadOnly]
        public NativeArray<int> PoseClipIds;

        [ReadOnly]
        public int CurrentClipId;

        [ReadOnly]
        public ETags FavourTags;

        [ReadOnly]
        public float FavourMultiplier;

        //Output
        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<int> ChosenPoseId;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<float> ChosenPoseCost;

        public void Execute(int startIndex, int count)
        {
            float bestCost = float.MaxValue;
            int bestPoseId = 0;

            for (int i = 0; i < count; ++i)
            {
                int index = startIndex + i;

                if (CurrentClipId == PoseClipIds[index])
                    continue;

                float curCost = (PoseCosts[index] + TrajCosts[index]) * PoseFavour[index];

                if ((PoseFavourTags[index] & FavourTags) != 0)
                    curCost *= FavourMultiplier;

                if (curCost < bestCost)
                {
                    bestCost = curCost;
                    bestPoseId = index;
                }
            }

            int batchId = startIndex / BatchSize;

            ChosenPoseId[batchId] = bestPoseId;
            ChosenPoseCost[batchId] = bestCost;
        }
    }//End of struct: FindMinima_FavourInclusive_EnforceClipChange

    //============================================================================================
    /**
    *  @brief Job struct used to find the minima of costs from a list of pose and trajectory 
    *  costs. The minima job is also responsible for applying pose favouring of any kind to the 
    *  costs before comparing.
    *  
    *  This FindMinima job also calculates favour tags into cost comparrisons. it is only used
    *  by the MxMAnimator when there are favoured tags.
    *         
    *********************************************************************************************/
    [BurstCompile(CompileSynchronously = true)]
    public struct FindMinima_FavourStacking : IJobParallelForBatch
    {
        //Input
        [ReadOnly]
        public NativeArray<float> PoseCosts;

        [ReadOnly]
        public NativeArray<float> TrajCosts;

        [ReadOnly]
        public NativeArray<float> PoseFavour;

        [ReadOnly]
        public NativeArray<ETags> PoseFavourTags;

        [ReadOnly]
        public int BatchSize;

        //Output
        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<int> ChosenPoseId;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<float> ChosenPoseCost;

        [ReadOnly]
        public ETags FavourTags;

        [ReadOnly]
        public float FavourMultiplier;

        public void Execute(int startIndex, int count)
        {
            float bestCost = float.MaxValue;
            int bestPoseId = 0;

            for (int i = 0; i < count; ++i)
            {
                int index = startIndex + i;

                float curCost = (PoseCosts[index] + TrajCosts[index]) * PoseFavour[index];

                ETags activeTags = PoseFavourTags[index] & FavourTags;
                uint activeTagCount = MxMUtility.CountFlags(activeTags);
                if (activeTagCount > 0)
                {
                    curCost *= math.pow(FavourMultiplier, activeTagCount);
                }
                

                if (curCost < bestCost)
                {
                    bestCost = curCost;
                    bestPoseId = index;
                }
            }

            int batchId = startIndex / BatchSize;

            ChosenPoseId[batchId] = bestPoseId;
            ChosenPoseCost[batchId] = bestCost;
        }
    }//End of struct: FindMinima_FavourStacking

    //============================================================================================
    /**
    *  @brief Job struct used to find the minima of costs from a list of pose and trajectory 
    *  costs. The minima job is also responsible for applying pose favouring of any kind to the 
    *  costs before comparing.
    *  
    *  This is the most complex FindMinima job and it contains all functionality from the base
    *  find Minima job while also applying favour tag multipliers and enforcing clip changes.
    *         
    *********************************************************************************************/
    [BurstCompile(CompileSynchronously = true)]
    public struct FindMinima_FavourStacking_EnforceClipChange : IJobParallelForBatch
    {
        //Input
        [ReadOnly]
        public NativeArray<float> PoseCosts;

        [ReadOnly]
        public NativeArray<float> TrajCosts;

        [ReadOnly]
        public NativeArray<float> PoseFavour;

        [ReadOnly]
        public NativeArray<ETags> PoseFavourTags;

        [ReadOnly]
        public int BatchSize;

        [ReadOnly]
        public NativeArray<int> PoseClipIds;

        [ReadOnly]
        public int CurrentClipId;

        [ReadOnly]
        public ETags FavourTags;

        [ReadOnly]
        public float FavourMultiplier;

        //Output
        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<int> ChosenPoseId;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<float> ChosenPoseCost;

        public void Execute(int startIndex, int count)
        {
            float bestCost = float.MaxValue;
            int bestPoseId = 0;

            for (int i = 0; i < count; ++i)
            {
                int index = startIndex + i;

                if (CurrentClipId == PoseClipIds[index])
                    continue;

                float curCost = (PoseCosts[index] + TrajCosts[index]) * PoseFavour[index];

                ETags activeTags = PoseFavourTags[index] & FavourTags;
                uint activeTagCount = MxMUtility.CountFlags(activeTags);
                if (activeTagCount > 0)
                {
                    curCost *= math.pow(FavourMultiplier, activeTagCount);
                }

                if (curCost < bestCost)
                {
                    bestCost = curCost;
                    bestPoseId = index;
                }
            }

            int batchId = startIndex / BatchSize;

            ChosenPoseId[batchId] = bestPoseId;
            ChosenPoseCost[batchId] = bestCost;
        }
    }//End of struct: FindMinima_FavourStacking_EnforceClipChange

}//End of namespace: MxM