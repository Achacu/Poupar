#ifndef FADEBYCAMDST_INCLUDED
#define FADEBYCAMDST_INCLUDED

void CalculateAlphaByCamDst_float(float _Visible, float _LowerBlindTh, float _UpperBlindTh, float3 camPos, float3 vertexPos, out float alpha)
{
    float dstToCam = length(camPos - vertexPos);//distance to camera

    //If _Visible is non-negative, that will be the alpha (set from outside: i.e. objects that make sound)
    //If the object is closer to cam than _LowerBlindTh it is completely visible; if between both thresholds, a gradient; if above _UpperBlindTh, completely invisible 
    alpha = (_Visible >= 0) ? _Visible : (dstToCam > _UpperBlindTh) ? 0 : (dstToCam < _LowerBlindTh) ? 1
        : (_UpperBlindTh - dstToCam) / (_UpperBlindTh - _LowerBlindTh);
}

#endif