Camera Movement:
- Translation: Forward, left, back and right movements of the camera are enabled using Input.GetKey of the keys WASD respectively.
- Rotation: Pitch and yaw of the camera are employed with Input.GetAxis using mouse.
- Both translation and rotation are updated each frame.
- A world boundary made of four rectangle game objects are created to preven the camera from flying out of bounds
- Uses RigidBody and Sphere Collider to stop collision with the terrain, water and world boundaries

Water:
- Increasing the number of vertices using a C# script is used to create realistic looking water.
- Movement: a shader is used to create water movements using both sine and cosine functions on the y axis and updating it with _Time.y
- Texture: Map a seamless water texture in the script synchronize it with the water movement inside the shader.
- Lighting: Apply Blinn-Phong shading to the water and include it inside the same shader for the movements, to avoid using two materials for one object.

Sun:
- A simple sphere that acts as a point light to give realistic lighting for the terrain and water
- Rotates around the terrain by utillizing Mathf.cos for x axis and Mathf.sin for y and z axis and multiplying each with Time.deltaTime and radius of rotation.
- Lighting of the water and terrain changes as the sun rotates by putting the sun script inside both respective scripts.

Landscape:
-

Landscape Properties:
- Color:
- Light