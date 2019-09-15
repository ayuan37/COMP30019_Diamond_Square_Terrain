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

Terrain:
- Generate vertices by dividing a square of size n x n into smaller divisions forming an array of vertices
- i.e. a size n x n square with 128 division, would have (128 + 1) x (128 + 1) number of vertices
- Diamond Square:
    - Randomly generate height for the four corners
        - Top Left and Top Rights corners are set to be above sea level (ie y > 0)
        - Bot Left and Bot Rights corners are set to be below sea level (ie y < 0)
        - So some water is alwasy visible
    - Run Log2(nDivisions) number of iterations, to make sure we go over every square of every possible size in the big n x n square
        - A square of 4 x 4 contains 5 squares of 2 different sizes (1 big + 4 small) and log2(4) = 2
            - in the first iteration, do diamond-square once for the big overall square
            - in the second iteration, do diamond-square 4 times for the 4 smaller size square
    - For each square:
        * - - x - - *  
        | - - - - - |
        | - - - - - |
        x - - m - - x Diamond, set m to be ave of * points +/- Random value
        | - - - - - | Square, set x to be ave of nearest 2 * points + m point +/- Random value
        | - - - - - |
        * - - x - - *
- An array of triangles is generated from the array of vertices of various heights, both are then used to create the mesh

Landscape Properties:
- Colour:
    - Done using vertex colours
    - Colour above the sea level is set by normalising the height of the terrain and map with colour on a gradient scale
        - Gradient goes like: white -> dark brown -> varies shades of green
    - Colour slightly above sea level and below sea level are set to yellowish, to model the colour of sand
- Light:
    - Fragement shader 
    - Using the Phong shading code from the Lab
    - Unlike water, terrain has lower specN, Kd and Ks, to make is less reflective than water 