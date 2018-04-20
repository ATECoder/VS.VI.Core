''' <summary> Builds a channel list. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/21/2005" by="David" revision="1.0.1847.x"> Created. </history>
Public Class ChannelListBuilder
    Implements IDisposable

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Constructs this class. </summary>
    Public Sub New()
        MyBase.New()
        Me._channelListStringBuilder = New System.Text.StringBuilder(String.Empty)
    End Sub

    ''' <summary> Constructs this class. </summary>
    ''' <param name="channelList"> A channel list to initialize. </param>
    Public Sub New(ByVal channelList As String)
        Me.New()
        Me.ChannelList = channelList
    End Sub

    ''' <summary> Calls <see cref="M:Dispose(Boolean Disposing)"/> to cleanup. </summary>
    ''' <remarks> Do not make this method Overridable (virtual) because a derived class should not be
    ''' able to override this method. </remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        Me.Dispose(True)
        ' Take this object off the finalization(Queue) and prevent finalization code 
        ' from executing a second time.
        GC.SuppressFinalize(Me)
    End Sub

    ''' <summary> Gets or sets the dispose status sentinel of the base class.  This applies to the
    ''' derived class provided proper implementation. </summary>
    ''' <value> The is disposed. </value>
    Protected Property IsDisposed() As Boolean

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> true to release both managed and unmanaged resources; false to
    '''                          release only unmanaged resources. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
                Me._channelListStringBuilder?.Clear() : Me._channelListStringBuilder = Nothing
            End If
        Finally
            Me.IsDisposed = True
        End Try
    End Sub

#End Region

#Region " SHARED: CHANNEL LIST ELEMENT BUILDERS "

    ''' <summary> Constructs a channel list element. </summary>
    ''' <param name="memoryLocation"> Specifies the memory location. </param>
    ''' <returns> A list of. </returns>
    Public Shared Function BuildElement(ByVal memoryLocation As Int32) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, "M{0}", memoryLocation)
    End Function

    ''' <summary> Constructs a channel list element. </summary>
    ''' <param name="slotNumber">  Specifies the card slot number in the relay mainframe. </param>
    ''' <param name="relayNumber"> Specifies the relay number in the slot. </param>
    ''' <returns> A list of. </returns>
    Public Shared Function BuildElement(ByVal slotNumber As Int32, ByVal relayNumber As Int32) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, "{0}!{1}", slotNumber, relayNumber)
    End Function

    ''' <summary> Constructs a channel list element. </summary>
    ''' <param name="slotNumber">   Specifies the card slot number in the relay mainframe. </param>
    ''' <param name="rowNumber">    Specifies the row number of the relay. </param>
    ''' <param name="columnNumber"> Specifies the column number of the relay. </param>
    ''' <returns> The channel list element string. </returns>
    Public Shared Function BuildElement(ByVal slotNumber As Int32, ByVal rowNumber As Int32, ByVal columnNumber As Int32) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, "{0}!{1}!{2}", slotNumber, rowNumber, columnNumber)
    End Function

    ''' <summary> Builds a range. </summary>
    ''' <param name="fromChannelElement"> Specifies the starting channel element to add. </param>
    ''' <param name="toChannelElement">   Specifies the ending channel element to add. </param>
    ''' <returns> A String. </returns>
    Public Shared Function BuildRange(ByVal fromChannelElement As String, ByVal toChannelElement As String) As String
        Return $"{fromChannelElement}:{toChannelElement}"
    End Function


#End Region

#Region " ADD CHANNEL ELEMENT "

    ''' <summary> Adds a relay channel to the channel list. </summary>
    ''' <param name="channelElement"> Specifies the channel element to add. </param>
    Public Sub AddChannel(ByVal channelElement As String)
        If Me._elementCount > 0 Then
            Me._channelListStringBuilder.Append(",")
        End If
        Me._channelListStringBuilder.Append(channelElement)
        Me._elementCount += 1
    End Sub

    ''' <summary> Adds a memory location to the channel list. </summary>
    ''' <param name="memoryLocation"> Specifies the memory location. </param>
    Public Sub AddChannel(ByVal memoryLocation As Int32)
        Me.AddChannel(ChannelListBuilder.BuildElement(memoryLocation))
    End Sub

    ''' <summary> Adds a card relay channel to the channel list. </summary>
    ''' <param name="slotNumber">  Specifies the card slot number in the relay mainframe. </param>
    ''' <param name="relayNumber"> Specifies the relay number in the slot. </param>
    Public Sub AddChannel(ByVal slotNumber As Int32, ByVal relayNumber As Int32)
        Me.AddChannel(ChannelListBuilder.BuildElement(slotNumber, relayNumber))
    End Sub

    ''' <summary> Adds a matrix relay channel to the channel list. </summary>
    ''' <param name="slotNumber">   Specifies the card slot number in the relay mainframe. </param>
    ''' <param name="rowNumber">    Specifies the row number of the relay. </param>
    ''' <param name="columnNumber"> Specifies the column number of the relay. </param>
    Public Sub AddChannel(ByVal slotNumber As Int32, ByVal rowNumber As Int32, ByVal columnNumber As Int32)
        Me.AddChannel(ChannelListBuilder.BuildElement(slotNumber, rowNumber, columnNumber))
    End Sub

    ''' <summary> Adds a range of relay channels to the channel list. </summary>
    ''' <param name="fromChannelElement"> Specifies the starting channel element to add. </param>
    ''' <param name="toChannelElement">   Specifies the ending channel element to add. </param>
    Public Sub AddChannelRange(ByVal fromChannelElement As String, ByVal toChannelElement As String)
        If Me._elementCount > 0 Then
            Me._channelListStringBuilder.Append(",")
        End If
        Me._channelListStringBuilder.Append(ChannelListBuilder.BuildRange(fromChannelElement, toChannelElement))
        Me._elementCount += 1
    End Sub

    ''' <summary> Adds a card relay range of channels channel to the channel list. </summary>
    ''' <param name="fromSlotNumber">  Specifies the starting card slot number in the relay
    ''' mainframe. </param>
    ''' <param name="fromRelayNumber"> Specifies the starting relay number in the slot. </param>
    ''' <param name="toSlotNumber">    Specifies the ending card slot number in the relay mainframe. </param>
    ''' <param name="toRelayNumber">   Specifies the ending relay number in the slot. </param>
    Public Sub AddChannelRange(ByVal fromSlotNumber As Int32, ByVal fromRelayNumber As Int32,
                               ByVal toSlotNumber As Int32, ByVal toRelayNumber As Int32)
        Me.AddChannelRange(ChannelListBuilder.BuildElement(fromSlotNumber, fromRelayNumber),
                           ChannelListBuilder.BuildElement(toSlotNumber, toRelayNumber))
    End Sub

    ''' <summary> Adds a matrix relay range of channels to the channel list. </summary>
    ''' <param name="fromSlotNumber">   Specifies the starting card slot number in the relay
    ''' mainframe. </param>
    ''' <param name="fromRowNumber">    Specifies the starting row number of the relay. </param>
    ''' <param name="fromColumnNumber"> from column number. </param>
    ''' <param name="toSlotNumber">     Specifies the ending card slot number in the relay mainframe. </param>
    ''' <param name="toRowNumber">      Specifies the ending row number of the relay. </param>
    ''' <param name="toColumnNumber">   Specifies the ending column number of the relay. </param>
    Public Sub AddChannel(ByVal fromSlotNumber As Int32, ByVal fromRowNumber As Int32, ByVal fromColumnNumber As Int32,
                          ByVal toSlotNumber As Int32, ByVal toRowNumber As Int32, ByVal toColumnNumber As Int32)
        Me.AddChannelRange(ChannelListBuilder.BuildElement(fromSlotNumber, fromRowNumber, fromColumnNumber),
                           ChannelListBuilder.BuildElement(toSlotNumber, toRowNumber, toColumnNumber))
    End Sub

    ''' <summary> The channel list string builder. </summary>
    Private _ChannelListStringBuilder As System.Text.StringBuilder

    ''' <summary> Gets or sets the channel list sans the '(@' prefix and ')' suffix. </summary>
    ''' <value> A List of naked channels. </value>
    Public Property NakedChannelList() As String
        Get
            Return Me._channelListStringBuilder.ToString
        End Get
        Set(ByVal value As String)
            Me._channelListStringBuilder = New System.Text.StringBuilder(value)
            Me._elementCount = Me._channelListStringBuilder.ToString.Split(","c).Length
        End Set
    End Property

    ''' <summary> Gets or sets an operation-able channel list that can be passed to the route and scan
    ''' commands. </summary>
    ''' <remarks> The channel list specifies the channels to be closed or opened.  Each comma delimited
    ''' channel in the list is made up of either a exclamation point separated two Int32 card relay
    ''' number (i.e., slot#!relay#) or a three Int32 exclamation point separated matrix relay number
    ''' (i.e., slot@!row#!column#) or a memory location (M#).  The channel list begins with a prefix
    ''' '(@' and ends with a suffix ")". </remarks>
    ''' <value> A List of channels. </value>
    Public Property ChannelList() As String
        Get
            Return String.Format(Globalization.CultureInfo.CurrentCulture, "(@{0})", Me._channelListStringBuilder)
        End Get
        Set(ByVal value As String)
            If Not String.IsNullOrWhiteSpace(value) Then
                Me.NakedChannelList = value.Substring(2).TrimEnd(")"c).Trim
            End If
        End Set
    End Property

    ''' <summary> Number of elements. </summary>
    Private _ElementCount As Int32

    ''' <summary> Returns the number of elements in the channel list.  This is not the same as the
    ''' number of channels. </summary>
    ''' <value> The number of elements. </value>
    Public ReadOnly Property ElementCount() As Int32
        Get
            Return Me._elementCount
        End Get
    End Property

#End Region

End Class
