#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

out vec2 TexCoords;
out vec3 WorldPos;
out vec3 Normal;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

void main()
{
    TexCoords = aTexCoords;
    WorldPos = vec3(vec4(aPos, 1.0) * model);
    Normal = aNormal * mat3(transpose(inverse(model)));
    
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
}