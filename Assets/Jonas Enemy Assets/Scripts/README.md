# Read this if you want to use the TVBot

To use it and make it work you need to have the following:
- AI Navigation Package installed (https://docs.unity3d.com/Packages/com.unity.ai.navigation@2.0/manual/index.html)
- The Player Figure needs to be in its own layer (e.g. "Player")
- The Ground needs to be in its own layer (e.g. "Ground")
- The TVBot needs to have the NavMeshAgent component
- In the scene, there must be an empty GameObject, with a NavMeshSurface Component that is baking the NavMesh
- You need to have animations for the TVBot (Idle, Walk, Run, Attack, Die) and add the AnimatorController to the TVBot