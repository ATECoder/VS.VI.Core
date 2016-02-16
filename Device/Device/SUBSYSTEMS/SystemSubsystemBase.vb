''' <summary> Defines the contract that must be implemented by System Subsystem. </summary>
''' <license> (c) 2012 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="9/26/2012" by="David" revision="1.0.4652"> Created. </history>
Public MustInherit Class SystemSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="SystemSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="VI.StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

#End Region

#Region " INTERFACE "

    ''' <summary> Supports clear interface. </summary>
    ''' <returns> <c>True</c> if supports clearing the interface. </returns>
    Public Function SupportsClearInterface() As Boolean
        Return Me.Session.SupportsClearInterface
    End Function

    ''' <summary> Clears the interface. </summary>
    Public Sub ClearInterface()
        If Me.SupportsClearInterface Then Me.Session.ClearInterface()
    End Sub

    ''' <summary> Clears the device (SDC). </summary>
    Public Sub ClearDevice()
        Me.Session.ClearDevice()
    End Sub

#End Region

#Region " MEMORY MANAGEMENT "

    ''' <summary> Gets or sets the initialize memory command. </summary>
    ''' <value> The initialize memory command. </value>
    Protected Overridable ReadOnly Property InitializeMemoryCommand As String

    ''' <summary> Initializes battery backed RAM. This initializes trace, source list, user-defined
    ''' math, source-memory locations, standard save setups, and all call math expressions. </summary>
    ''' <remarks> Sends the <see cref="InitializeMemoryCommand"/> message. </remarks>
    Public Sub InitializeMemory()
        If Not String.IsNullOrWhiteSpace(Me.InitializeMemoryCommand) Then
            Me.Session.WriteLine(Me.InitializeMemoryCommand)
        End If
    End Sub

#End Region

End Class
