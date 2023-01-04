#ifndef CALCULATEALPHABYCOLLISION_INCLUDED
#define CALCULATEALPHABYCOLLISION_INCLUDED

//UNITY_INSTANCING_BUFFER_START(Props)
//// put more per-instance properties here
//float4 _ColPoints[30];
//UNITY_INSTANCING_BUFFER_END(Props)

void CalculateAlphaByCollision_float(float _OverrideAlpha,float _Colliding, float _Sounding, float _ColAreaRadius, float3 vertexPos, float4 colPoint0,float4 colPoint1,float4 colPoint2,float4 colPoint3,float4 colPoint4, float4 colPoint5, float4 colPoint6, float4 colPoint7, float4 colPoint8, float4 colPoint9, out float alpha)
{
    float4 _ColPoints[10] = { colPoint0, colPoint1, colPoint2, colPoint3, colPoint4, colPoint5, colPoint6, colPoint7, colPoint8, colPoint9 };
    float dstToHitPoint = 0; 
    bool closeToColPos = false;

    //The loop is aborted when there's no collision or the current point is within reach of a colPoint.
    for (int i = 0; (_Colliding != 0) && (i < 10) && !closeToColPos; i++) 
    {
        //1st check avoid calculating distance to null positions
        dstToHitPoint = (_ColPoints[i].w == 0)? 100: /*(_ColPoints[i].xyz == float3(0, 0, 0)) ? 100 :*/ distance(_ColPoints[i].xyz, vertexPos);
        closeToColPos = (dstToHitPoint < _ColAreaRadius);
    }
    alpha = (_OverrideAlpha >= 0) ? _OverrideAlpha :
        closeToColPos ? max(_Colliding, _Sounding) : _Sounding;
}

#endif