#version 440 core

layout(location = 0) in vec2 pos;
layout(location = 1) in vec4 col;
layout(location = 2) in vec2 texCoord;
layout(location = 3) uniform mat4 transform;

out vec4 fragColor;

void main(void){
	fragColor = col;
	gl_Position = transform * vec4(pos, 0.0, 1.0);
}
