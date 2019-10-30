#version 400

layout(location = 0) in vec3 in_Position; 
layout(location = 1) in vec4 in_Color; 

varying vec4 ex_Color;

uniform mat3 transform;

void main(void)
{ 
	ex_Color = in_Color;
	gl_Position = vec4((transform * vec3(in_Position.xy, 1.0f)).xy, in_Position.z, 1.0f);

}