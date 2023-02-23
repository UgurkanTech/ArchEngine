#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

out vec3 Normal;
out vec3 FragPos;
out vec2 TexCoords;

out vec3 Tangent;
out vec3 Bitangent;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
    
    FragPos = vec3(vec4(aPos, 1.0) * model);
    Normal = aNormal * mat3(transpose(inverse(model)));
    TexCoords = aTexCoords;

    vec3 T, B, N;
    T = normalize(vec3(model * vec4(cross(vec3(0, 1, 0), aNormal), 0.0)));
    B = normalize(vec3(model * vec4(cross(aNormal, T), 0.0)));
    //N = normalize(vec3(model * vec4(aNormal, 0.0)));

    Tangent = T;
    Bitangent = B;
    
}
