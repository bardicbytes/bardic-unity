using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace BardicBytes.BardicUnity.Editor
{
    public class BardicStateMachineWindow : EditorWindow
    {
        private List<BardicStateMachine> machines;

        [MenuItem("Bardic/State Machine Window")]
        public static void CreateWindow()
        {
            var w = GetWindow<BardicStateMachineWindow>(true);
            w.titleContent = new GUIContent("Bardic State Machine Window");
        }

        private void CreateGUI()
        {
            var foundMachines = FindObjectsByType<BardicStateMachine>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            machines = new List<BardicStateMachine>(foundMachines);

            VisualElement root = rootVisualElement;

            ScrollView scrollView = new ScrollView();
            scrollView.style.flexGrow = 1;
            root.Add(scrollView);

            foreach (BardicStateMachine machine in machines)
            {
                Button button = new Button(() => SelectStateMachine(machine))
                {
                    text = "Select " + machine.name 
                };

                scrollView.Add(button);
            }
        }

        private void SelectStateMachine(BardicStateMachine machine)
        {
            Selection.activeObject = machine;
        }

    }
}
