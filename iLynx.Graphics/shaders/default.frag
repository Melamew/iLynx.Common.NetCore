#version 440 core

in vec4 fragColor;
in vec2 textureCoordinate;

layout(location = 0) out vec4 color;

void main(void) {
//	color = vec4(1.0f, 0.0f, 0.0f, 1.0f);
	color = fragColor;
//	gl_FragColor = fragColor;
}
