#version 330

uniform sampler2D offScreenTexture;
uniform float effectScale = 0.3;

in vec2 uv;
uniform vec3 circleInformation;
out vec4 colorOut;

#define opacity 0.2

float circle(vec2 coord, float startFadeOut, float endFadeOut)
{
	float dist = distance(vec2(0.5), coord);
	return 1.0 - smoothstep(startFadeOut, endFadeOut, dist);
}


void main(){
	float pct = 0.0;
	vec2 uv3 = uv.xy - 0.5;
	pct = distance(uv,vec2(0.5));

	
	colorOut = vec4(1, 0, 0, circle(uv, circleInformation.x, circleInformation.y+circleInformation.z)); 
}

/*
void mainmeh()
{
	vec2 uv3 = uv.xy / windowSize.xy;
   
    uv3 *=  1.0 - uv3.yx;   //vec2(1.0)- uv.yx; -> 1.-u.yx; Thanks FabriceNeyret !
    
    float vig = uv3.x*uv3.y * 15.0; // multiply with sth for intensity
    
    vig = pow(vig, 0.25); // change pow for modifying the extend of the  vignette

    
    colorOut = vec4(vig); 
}


void main1212() {
	vec4 image = vec4(texture(offScreenTexture, uv).rgb, 1.0f);
	 // Center the UV
    vec2 centered = uv - vec2(0.5);
    
    vec4 color = vec4(1.0);
    
    // Create the vignette effect in black and white
    color.rgb *= 1.0 - smoothstep(circleInformation.x, circleInformation.y, length(centered));
    
    // Apply the vignette to the image
    color *= image;
    
    // Mix between the vignette version and the original to change the vignette opacity
    color = mix(image, color, opacity);
    
    colorOut = color;
}


void main2() {
	vec3 text = texture(offScreenTexture, uv).rgb;

	
	//color = vec3(1.-dot(uv*0.5,uv*0.45), 0f, 0f);
	//float startFadeOut = 0.6 - clamp(effectScale, 0.0, 1.0);
	//float smoothness = 0.5;
	//color *= circle(uv, startFadeOut, startFadeOut + smoothness);

	//gl_FragColor = vec4(color, 1.0);
	//gl_FragColor = vec4(1.0, 1.0, 1.0, 0.5);
	vec3 color;
	if(uv.x > 0.3f){

	//color = vec3(1f, 0f, 0f);
	color = vec3(1.-dot(uv*0.5,uv*0.45), 0f, 0f);
	}
	else{
	//color = vec3(1f, 1f, 0f);
	}
	//colorOut = vec4(color, 0.2f);
	
    vec2 coord = (uv - 0.5) * (iResolution.x/iResolution.y) * 2.0;
    float rf = sqrt(dot(coord, coord)) * Falloff;
    float rf2_1 = rf * rf + 1.0;
    float e = 1.0 / (rf2_1 * rf2_1);
    
    vec4 src = vec4(1.0,1.0,1.0,1.0);
	colorOut = vec4(src.rgb * e, 1.0);
}


uniform sampler2D offScreenTexture;
uniform float effectScale = 0.3;

in vec2 uv;
in vec2 radius;
out vec4 colorOut;

float circle(vec2 coord, float startFadeOut, float endFadeOut)
{
	float dist = distance(vec2(0.5), coord);
	return 1.0 - smoothstep(startFadeOut, endFadeOut, dist);
}

void main() {
	 // Center the UV
    vec2 centered = uv - vec2(0.5);
    
    vec4 color = vec4(1.0);
    
    // Create the vignette effect in black and white
    color.rgb *= 1.0 - smoothstep(radius.x, radius.y, length(centered));
    
    // Apply the vignette to the image
    color *= image;
    
    // Mix between the vignette version and the original to change the vignette opacity
    color = mix(image, color, opacity);
    
    vec4 src = vec4(1.0,1.0,1.0,1.0);
	colorOut = vec4(src.rgb * e, 1.0);
}
*/