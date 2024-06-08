using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(RunManager))]
public class RunManagerInspector : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        // Create a new VisualElement to be the root of our Inspector UI.
        VisualElement myInspector = new VisualElement();
        InspectorElement.FillDefaultInspector( myInspector, serializedObject, this );

        // Add a simple label.
        var resetButton = new Button(OnResetPress) { text = "Reset Run" };
        var completeButton = new Button(OnCompletePress) { text = "Complete World" };
        var saveButton = new Button(OnSavePress) { text = "Save State" };
        var deleteButton = new Button(OnDeletePress) { text = "Delete State" };

        resetButton.style.marginTop = 32;
        saveButton.style.marginTop = 32;

        resetButton.style.height = 32;
        completeButton.style.height = 32;
        saveButton.style.height = 32;
        deleteButton.style.height = 32;
        
        myInspector.Add(resetButton);
        myInspector.Add(completeButton);
        myInspector.Add(saveButton);
        myInspector.Add(deleteButton);

        // Return the finished Inspector UI.
        return myInspector;
    }
    
    private void OnResetPress()
    {
        (target as RunManager).ResetRun();
    }

    private void OnCompletePress()
    {
        (target as RunManager).OnLevelComplete();
    }
    
    private void OnSavePress()
    {
        (target as RunManager).SavePlayerProgress();

    }
    
    private void OnDeletePress()
    {
        (target as RunManager).WipePlayerProgress();
    }
}
