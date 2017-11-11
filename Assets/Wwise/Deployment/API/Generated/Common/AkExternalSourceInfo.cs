#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.11
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */


using System;
using System.Runtime.InteropServices;

public class AkExternalSourceInfo : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal AkExternalSourceInfo(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(AkExternalSourceInfo obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  ~AkExternalSourceInfo() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          AkSoundEnginePINVOKE.CSharp_delete_AkExternalSourceInfo(swigCPtr);
        }
        swigCPtr = IntPtr.Zero;
      }
      GC.SuppressFinalize(this);
    }
  }

  public uint iExternalSrcCookie {
    set {
      AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_iExternalSrcCookie_set(swigCPtr, value);
    } 
    get {
      uint ret = AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_iExternalSrcCookie_get(swigCPtr);
      return ret;
    } 
  }

  public uint idCodec {
    set {
      AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_idCodec_set(swigCPtr, value);
    } 
    get {
      uint ret = AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_idCodec_get(swigCPtr);
      return ret;
    } 
  }

  public string szFile {	set { AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_szFile_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_szFile_get(swigCPtr); } 
  }

  public IntPtr pInMemory { set { AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_pInMemory_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_pInMemory_get(swigCPtr); }
  }

  public uint uiMemorySize {
    set {
      AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_uiMemorySize_set(swigCPtr, value);
    } 
    get {
      uint ret = AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_uiMemorySize_get(swigCPtr);
      return ret;
    } 
  }

  public uint idFile {
    set {
      AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_idFile_set(swigCPtr, value);
    } 
    get {
      uint ret = AkSoundEnginePINVOKE.CSharp_AkExternalSourceInfo_idFile_get(swigCPtr);
      return ret;
    } 
  }

  public AkExternalSourceInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkExternalSourceInfo__SWIG_0(), true) {
  }

  public AkExternalSourceInfo(IntPtr in_pInMemory, uint in_uiMemorySize, uint in_iExternalSrcCookie, uint in_idCodec) : this(AkSoundEnginePINVOKE.CSharp_new_AkExternalSourceInfo__SWIG_1(in_pInMemory, in_uiMemorySize, in_iExternalSrcCookie, in_idCodec), true) {
  }

  public AkExternalSourceInfo(string in_pszFileName, uint in_iExternalSrcCookie, uint in_idCodec) : this(AkSoundEnginePINVOKE.CSharp_new_AkExternalSourceInfo__SWIG_2(in_pszFileName, in_iExternalSrcCookie, in_idCodec), true) {
  }

  public AkExternalSourceInfo(uint in_idFile, uint in_iExternalSrcCookie, uint in_idCodec) : this(AkSoundEnginePINVOKE.CSharp_new_AkExternalSourceInfo__SWIG_3(in_idFile, in_iExternalSrcCookie, in_idCodec), true) {
  }

}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.