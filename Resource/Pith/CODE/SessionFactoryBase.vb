''' <summary> A session factory base. </summary>
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
Public MustInherit Class SessionFactoryBase

    Protected Sub New()
        MyBase.New
    End Sub

    ''' <summary> Creates Gpib interface SessionBase. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    ''' <returns> The new Gpib interface SessionBase. </returns>
    Public MustOverride Function CreateGpibInterfaceSession() As InterfaceSessionBase

    ''' <summary> Creates resources manager. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    ''' <returns> The new resources manager. </returns>
    Public MustOverride Function CreateResourcesManager() As ResourcesManagerBase

    ''' <summary> Creates the SessionBase. </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    ''' <returns> The new SessionBase. </returns>
    Public MustOverride Function CreateSession() As SessionBase

End Class
