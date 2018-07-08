#version 440 core

layout(location = 0) in vec3 pos;
layout(location = 1) in vec2 texCoord;
layout(location = 2) in vec4 col;


layout(location = 3) uniform mat4 transform;

out vec4 fragColor;
out vec2 textureCoordinate;

void main(void){
    fragColor = col;
    textureCoordinate = texCoord;
    gl_Position = transform * vec4(pos, 1.0);
}
