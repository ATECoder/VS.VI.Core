''' <summary> A session factory. </summary>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="11/29/2015" by="David" revision=""> Created. </history>
Public Class SessionFactory
    Inherits VI.Pith.SessionFactoryBase

    ''' <summary> Creates gpib interface session. </summary>
    ''' <returns> The new gpib interface session. </returns>
    Public Overrides Function CreateGpibInterfaceSession() As VI.Pith.InterfaceSessionBase
        Return New GpibInterfaceSession()
    End Function

    ''' <summary> Creates resources manager. </summary>
    ''' <returns> The new resources manager. </returns>
    Public Overrides Function CreateResourcesManager() As Pith.ResourcesManagerBase
        Return New LocalResourcesManager()
    End Function

    ''' <summary> Creates a session. </summary>
    ''' <returns> The new session. </returns>
    Public Overrides Function CreateSession() As Pith.SessionBase
        Return New Session()
    End Function

End Class
