#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;
layout (location = 3) in vec3 aTangent;
layout (location = 4) in vec3 aBitangent;

out VS_OUT {
    vec3 FragPos; //WorldPos
    vec2 TexCoords;
    vec3 TangentViewPos;
    vec3 TangentFragPos;
    vec3 Normal;
    vec3 camPos;
    mat3 tangentSpace;
} vs_out;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;


uniform vec3 viewPos;

void main()
{
    vs_out.FragPos = vec3(vec4(aPos, 1.0) * model);
    vs_out.TexCoords = aTexCoords;

    vec3 T = normalize(aTangent * mat3(model));
    vec3 B = normalize(aBitangent * mat3(model));
    vec3 N = normalize(aNormal * mat3(model));
    mat3 TBN = transpose(mat3(T, B, N));
    vs_out.tangentSpace = TBN;
    vs_out.Normal = N;
    vs_out.camPos = viewPos;
    //vs_out.TangentLightPos = TBN * lightPos;
    vs_out.TangentViewPos  = TBN * viewPos;
    vs_out.TangentFragPos  = TBN * vs_out.FragPos;
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
}