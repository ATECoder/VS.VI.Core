''' <summary> A session factory. </summary>
''' <remarks> David, 11/29/2015. </remarks>
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
    Inherits SessionFactoryBase

    ''' <summary> Creates gpib interface session. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    ''' <returns> The new gpib interface session. </returns>
    Public Overrides Function CreateGpibInterfaceSession() As InterfaceSessionBase
        Return New GpibInterfaceSession()
    End Function

    ''' <summary> Creates resources manager. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    ''' <returns> The new resources manager. </returns>
    Public Overrides Function CreateResourcesManager() As ResourcesManagerBase
        Return New LocalResourcesManager()
    End Function

    ''' <summary> Creates a session. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    ''' <returns> The new session. </returns>
    Public Overrides Function CreateSession() As SessionBase
        Return New Session()
    End Function

End Class
