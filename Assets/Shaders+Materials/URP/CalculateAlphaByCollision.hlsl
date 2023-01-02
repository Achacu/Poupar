#ifndef CALCULATEALPHABYCOLLISION_INCLUDED
#define CALCULATEALPHABYCOLLISION_INCLUDED

float4 _ColPoints[15];

void CalculateAlphaByCollision_float(float _OverrideAlpha,float _Colliding, float _Sounding, float _ColAreaRadius, float3 vertexPos, out float alpha)
{
    float dstToHitPoint = 0; 
    bool closeToColPos = false;

    //The loop is aborted when there's no collision or the current point is within reach of a colPoint.
    for (int i = 0; (_Colliding != 0) && (i < 15) && !closeToColPos; i++) 
    {
        //1st check avoid calculating distance to null positions
        dstToHitPoint = (_ColPoints[i].xyz == float3(0, 0, 0)) ? 100 : distance(_ColPoints[i], vertexPos);
        closeToColPos = (dstToHitPoint < _ColAreaRadius);
    }
    alpha = (_OverrideAlpha >= 0) ? _OverrideAlpha :
        closeToColPos ? max(_Colliding, _Sounding) : _Sounding;
}

#endif