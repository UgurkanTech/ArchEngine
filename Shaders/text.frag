#version 330
in vec2 vUV;
uniform sampler2D u_texture;
uniform vec3 textColor;
out vec4 fragColor;
void main()
{
    vec2 uv = vUV.xy;
    float text = texture(u_texture, uv).r;
    fragColor = vec4(textColor.rgb*text, text);
}