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

    ''' <summary>
    ''' Constructor that prevents a default instance of this class from being created.
    ''' </summary>
    ''' <remarks> David, 11/29/2015. </remarks>
    Private Sub New()
        MyBase.New
    End Sub

    ''' <summary>
    ''' The locking object to enforce thread safety when creating the singleton instance.
    ''' </summary>
    Private Shared ReadOnly syncLocker As New Object

    ''' <summary>
    ''' The shared instance.
    ''' </summary>
    Private Shared _instance As SessionFactory

    ''' <summary> Returns a new or existing instance of this class. </summary>
    ''' <remarks> Use this property to get an instance of this class. Returns the default instance of
    ''' this form allowing to use this form as a singleton for. </remarks>
    ''' <value> <c>A</c> new or existing instance of the class. </value>
    Public Shared ReadOnly Property [Get]() As SessionFactory
        Get
            If SessionFactory._instance Is Nothing Then
                SyncLock SessionFactory.syncLocker
                    SessionFactory._instance = New SessionFactory
                    SessionFactory._instance.UseNationalVisaSessionFactory()
                End SyncLock
            End If
            Return SessionFactory._instance
        End Get
    End Property

    ''' <summary> Gets or sets the factory. </summary>
    ''' <value> The factory. </value>
    Public Property Factory As SessionFactoryBase

End Class
