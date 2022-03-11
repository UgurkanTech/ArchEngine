#version 330
in vec2 in_pos;
in vec2 in_uv;
out vec2 vUV;
uniform mat4 model;
uniform mat4 projection;
            
void main()
{
    vUV = in_uv.xy;
	gl_Position = projection * model * vec4(in_pos.xy, 0.0, 1.0);
}