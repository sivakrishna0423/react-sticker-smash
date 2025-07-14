using System;
using UnityEngine;

public class ExerciseDataManager
{
    public int HoldTime { get; private set; }
    public int NumberOfSets { get; private set; }
    public int NumberOfCycles { get; private set; }
    public int MaxPlays { get; private set; }
    public string StartTime { get; private set; }
    public string EndTime { get; private set; }
    public string Side { get; set; }

    private readonly string moduleName;
    private readonly PhysioExercisesAIG physioExercisesAIG;

    public ExerciseDataManager(string moduleName, PhysioExercisesAIG physioExercisesAIG)
    {
        this.moduleName = moduleName;
        this.physioExercisesAIG = physioExercisesAIG;
    }

    public void InitializeExerciseData()
    {
        if (PatientPackageInfo.instance != null)
        {
            PatientPackageInfo.instance.GetVariationOfExercise(moduleName);
            physioExercisesAIG.ModuleName = moduleName;
            HoldTime = PatientPackageInfo.instance.ExerciseHoldTime;
            NumberOfSets = PatientPackageInfo.instance.ExerciseSets;
            NumberOfCycles = PatientPackageInfo.instance.ExerciseRep;
            MaxPlays = NumberOfSets * NumberOfCycles;
        }
        else
        {
            // Default values
            HoldTime = 5;
            NumberOfSets = 2;
            NumberOfCycles = 2;
            MaxPlays = 4;
        }

        StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void SendDataToServer(int playCount)
    {
        EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        PatientPackageApiManager.instance.GetModuleDetails(moduleName);
        physioExercisesAIG.SendingtoServerAllmodules(
            moduleName, 
            physioExercisesAIG.imInworkoutindex, 
            StartTime, 
            NumberOfCycles, 
            HoldTime, 
            0, 
            playCount, 
            NumberOfCycles, 
            NumberOfSets, 
            NumberOfSets, 
            Side
        );
    }
}