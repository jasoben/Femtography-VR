using UnityEditor;
using UnityEngine;

namespace WaveMaker
{
    public class WaveMakerMenu : MonoBehaviour
    {
        [MenuItem("GameObject/3D Object/WaveMaker/WaveMaker Surface", false)]
        static void CreateWaveMakerObject(MenuCommand menuCommand)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "WaveMaker Surface";
            go.GetComponent<MeshFilter>().sharedMesh = null;
            go.GetComponent<MeshFilter>().mesh = null;
            go.AddComponent<WaveMakerSurface>();
            go.GetComponent<Collider>().isTrigger = true;

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create WaveMaker Object " + go.name);
            Debug.Log("WaveMaker GameObject created: " + go.name + ". Create and attach to it a WaveMaker Descriptor on the project folder");

            var mat = Resources.Load("Materials/WaveMakerWaveMaterial", typeof(Material)) as Material;
            go.GetComponent<MeshRenderer>().material = mat;

            Selection.activeObject = go;
        }

        [MenuItem("GameObject/3D Object/WaveMaker/WaveMaker Interactor", false)]
        static void CreateWaveMakerInteractor(MenuCommand menuCommand)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "WaveMaker Interactor";
            go.AddComponent<WaveMakerInteractor>();

            var collider = go.GetComponent<SphereCollider>();
            collider.isTrigger = true;
            var rb = go.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create WaveMaker Interactor " + go.name);
            Debug.Log("WaveMaker Interactor created: " + go.name);

            Selection.activeObject = go;
        }
    }
}