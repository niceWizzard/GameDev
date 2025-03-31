using Main.World.Objects.Barrel;
using Main.World.Objects.Traps;
using Main.World.Objects.Traps.Timed.TimedSpikes;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TrapController))]
    public class TrapHandleEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var trap = (TrapController)target;
            var trapTransform = trap.transform;

            // Enable Undo support
            Undo.RecordObject(trapTransform, "Move Trap");

            // Define grid size (16x16 pixels per step, assuming 100 pixels = 1 unit)
            var gridStep = 1; // Adjust based on your unit scale

            // Get object's sprite bounds
            var spriteRenderer = trap.GetComponent<SpriteRenderer>();
            if (!spriteRenderer) return;

            var bounds = spriteRenderer.bounds;
            var lowerLeftCorner = bounds.min; // Get the lower-left corner position

            // Show handle at the lower-left corner
            var newPosition = Handles.PositionHandle(lowerLeftCorner, Quaternion.identity);

            // Snap to 16x16 grid
            newPosition.x = Mathf.Round(newPosition.x / gridStep) * gridStep;
            newPosition.y = Mathf.Round(newPosition.y / gridStep) * gridStep;

            // Apply the snapped position by shifting the object
            var offset = lowerLeftCorner - trapTransform.position; 
            var adjustedPosition = newPosition - offset;

            if (trapTransform.position == adjustedPosition) return;
            trapTransform.position = adjustedPosition;
            EditorUtility.SetDirty(trap);
        }
    }
    [CustomEditor(typeof(TimedTrapController))]
    public class TimedTrapEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var trap = (TimedTrapController)target;
            var trapTransform = trap.transform;

            // Enable Undo support
            Undo.RecordObject(trapTransform, "Move Trap");

            // Define grid size (16x16 pixels per step, assuming 100 pixels = 1 unit)
            var gridStep = 1; // Adjust based on your unit scale

            // Get object's sprite bounds
            var spriteRenderer = trap.GetComponent<SpriteRenderer>();
            if (!spriteRenderer) return;

            var bounds = spriteRenderer.bounds;
            var lowerLeftCorner = bounds.min; // Get the lower-left corner position

            // Show handle at the lower-left corner
            var newPosition = Handles.PositionHandle(lowerLeftCorner, Quaternion.identity);

            // Snap to 16x16 grid
            newPosition.x = Mathf.Round(newPosition.x / gridStep) * gridStep;
            newPosition.y = Mathf.Round(newPosition.y / gridStep) * gridStep;

            // Apply the snapped position by shifting the object
            var offset = lowerLeftCorner - trapTransform.position; 
            var adjustedPosition = newPosition - offset;

            if (trapTransform.position == adjustedPosition) return;
            trapTransform.position = adjustedPosition;
            EditorUtility.SetDirty(trap);
        }
    }
    
    [CustomEditor(typeof(BarrelController))]
    public class BarrelHandleEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var trap = (BarrelController)target;
            var trapTransform = trap.transform;

            // Enable Undo support
            Undo.RecordObject(trapTransform, "Move Trap");

            // Define grid size (16x16 pixels per step, assuming 100 pixels = 1 unit)
            var gridStep = 1; // Adjust based on your unit scale

            // Get object's sprite bounds
            var spriteRenderer = trap.GetComponent<SpriteRenderer>();
            if (!spriteRenderer) return;

            var bounds = spriteRenderer.bounds;
            var lowerLeftCorner = bounds.min; // Get the lower-left corner position

            // Show handle at the lower-left corner
            var newPosition = Handles.PositionHandle(lowerLeftCorner, Quaternion.identity);

            // Snap to 16x16 grid
            newPosition.x = Mathf.Round(newPosition.x / gridStep) * gridStep;
            newPosition.y = Mathf.Round(newPosition.y / gridStep) * gridStep;

            // Apply the snapped position by shifting the object
            var offset = lowerLeftCorner - trapTransform.position; 
            var adjustedPosition = newPosition - offset;

            if (trapTransform.position == adjustedPosition) return;
            trapTransform.position = adjustedPosition;
            EditorUtility.SetDirty(trap);
        }
    }
    
    
}
