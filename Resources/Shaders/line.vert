#version 330 core

layout(location = 0) in vec3 aPosition;



uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec4 color;

out vec4 clr;

void main(void)
{
    clr = color;
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}