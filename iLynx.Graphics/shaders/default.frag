#version 440 core

in vec4 fragColor;
in vec2 textureCoordinate;

out vec4 color;

uniform sampler2D m_texture;

void main(void) {
//  color = vec4(1.0f, 0.0f, 0.0f, 1.0f);
    vec4 texColor = texture(m_texture, textureCoordinate);
    color = fragColor * texColor;
//    color = clamp(fragColor + texColor, 0, 1);
//  gl_FragColor = fragColor;
}
