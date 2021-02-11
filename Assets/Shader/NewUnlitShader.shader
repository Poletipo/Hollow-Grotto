Shader "Test/TestShader"{
	Properties{
		_Color("Color", Color) = (0,0,0,1)
	}

	SubShader{

		Pass{

			Material{
				Diffuse[_Color]
			}
			Lighting On

		}

	}
}