''' <summary> Defines the contract that must be implemented by a Route Subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="01/15/2005" by="David" revision="1.0.1841.x"> Created. </history>
Public MustInherit Class RouteSubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTORS  and  DESTRUCTORS "

    ''' <summary> Initializes a new instance of the <see cref="RouteSubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
        Me._ScanList = ""
    End Sub

#End Region

#Region " I PRESETTABLE "

    ''' <summary> Sets the subsystem to its reset state. </summary>
    Public Overrides Sub ResetKnownState()
        MyBase.ResetKnownState()
        Me.ScanList = ""
    End Sub

#End Region

#Region " CLOSED CHANNEL "

    Private _ClosedChannel As String

    ''' <summary> Gets or sets the closed Channel. </summary>
    ''' <remarks> Nothing is not set. </remarks>
    ''' <value> The closed Channel. </value>
    Public Overloads Property ClosedChannel As String
        Get
            Return Me._ClosedChannel
        End Get
        Protected Set(ByVal value As String)
            If Not String.Equals(value, Me.ClosedChannel) Then
                Me._ClosedChannel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Applies the closed Channel described by value. </summary>
    ''' <param name="value">   The scan list. </param>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function ApplyClosedChannel(ByVal value As String, ByVal timeout As TimeSpan) As String
        Me.WriteClosedChannel(value, timeout)
        Return Me.QueryClosedChannel()
    End Function

    ''' <summary> Gets the closed Channel query command. </summary>
    ''' <value> The closed Channel query command. </value>
    ''' <remarks> :ROUT:CLOS </remarks>
    Protected Overridable ReadOnly Property ClosedChannelQueryCommand As String

    ''' <summary> Queries closed Channel. </summary>
    ''' <returns> The closed Channel. </returns>
    Public Function QueryClosedChannel() As String
        Me.ClosedChannel = Me.Query(Me.ClosedChannel, Me.ClosedChannelQueryCommand)
        Return Me.ClosedChannel
    End Function

    ''' <summary> Gets the closed Channel command format. </summary>
    ''' <value> The closed Channel command format. </value>
    ''' <remarks> :ROUT:CLOS {0} </remarks>
    Protected Overridable ReadOnly Property ClosedChannelCommandFormat As String

    ''' <summary> Writes a closed Channel. </summary>
    ''' <param name="value"> The scan list. </param>
    ''' <returns> A String. </returns>
    Public Function WriteClosedChannel(ByVal value As String, ByVal timeout As TimeSpan) As String
        Me.Write(Me.ClosedChannelCommandFormat, value)
        If timeout > TimeSpan.Zero Then Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        Me.ClosedChannel = value
        Me.ClosedChannels = Nothing
        Return Me.ClosedChannel
    End Function

    ''' <summary> Gets the open Channel command format. </summary>
    ''' <value> The open Channel command format. </value>
    ''' <remarks> :ROUT:OPEN:ALL </remarks>
    Protected Overridable ReadOnly Property OpenChannelCommandFormat As String

    ''' <summary> Applies the open channel list and reads back the list. </summary>
    ''' <param name="channelList"> List of Channel. </param>
    ''' <param name="timeout">     The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function ApplyOpenChannel(ByVal channelList As String, ByVal timeout As TimeSpan) As String
        Me.WriteOpenChannel(channelList, timeout)
        Return Me.QueryClosedChannel()
        Return Me.ClosedChannel
    End Function

    ''' <summary> Opens the specified Channel in the list. </summary>
    ''' <exception cref="ArgumentNullException" guarantee="strong"> . </exception>
    ''' <param name="channelList"> List of Channel. </param>
    Public Function WriteOpenChannel(ByVal channelList As String, ByVal timeout As TimeSpan) As String
        Me.Session.Execute(String.Format(Me.OpenChannelCommandFormat, channelList))
        If timeout > TimeSpan.Zero Then Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        ' set to nothing to indicate that the value is not known -- requires reading.
        Me.ClosedChannel = Nothing
        Me.ClosedChannels = Nothing
        Return Me.ClosedChannel
    End Function

#End Region

#Region " CLOSED CHANNELS "

    Private _ClosedChannels As String

    ''' <summary> Gets or sets the closed channels. </summary>
    ''' <remarks> Nothing is not set. </remarks>
    ''' <value> The closed channels. </value>
    Public Overloads Property ClosedChannels As String
        Get
            Return Me._ClosedChannels
        End Get
        Protected Set(ByVal value As String)
            If Not String.Equals(value, Me.ClosedChannels) Then
                Me._ClosedChannels = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Applies the closed channels described by value. </summary>
    ''' <param name="value">   The scan list. </param>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function ApplyClosedChannels(ByVal value As String, ByVal timeout As TimeSpan) As String
        Me.WriteClosedChannels(value, timeout)
        Return Me.QueryClosedChannels()
    End Function

    ''' <summary> Gets the closed channels query command. </summary>
    ''' <value> The closed channels query command. </value>
    ''' <remarks> :ROUT:CLOS </remarks>
    Protected Overridable ReadOnly Property ClosedChannelsQueryCommand As String

    ''' <summary> Queries closed channels. </summary>
    ''' <returns> The closed channels. </returns>
    Public Function QueryClosedChannels() As String
        Me.ClosedChannels = Me.Query(Me.ClosedChannels, Me.ClosedChannelsQueryCommand)
        Return Me.ClosedChannels
    End Function

    ''' <summary> Gets the closed channels command format. </summary>
    ''' <value> The closed channels command format. </value>
    ''' <remarks> :ROUT:CLOS {0} </remarks>
    Protected Overridable ReadOnly Property ClosedChannelsCommandFormat As String

    ''' <summary> Writes a closed channels. </summary>
    ''' <param name="value">   The scan list. </param>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function WriteClosedChannels(ByVal value As String, ByVal timeout As TimeSpan) As String
        Me.Write(Me.ClosedChannelsCommandFormat, value)
        If timeout > TimeSpan.Zero Then Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        Me.ClosedChannels = value
        ' set to nothing to indicate that the value is not known -- requires reading.
        Me.ClosedChannel = Nothing
        Me.OpenChannels = Nothing
        Return Me.ClosedChannels
    End Function

#End Region

#Region " OPEN CHANNELS "

    Private _OpenChannels As String

    ''' <summary> Gets or sets the Open channels. </summary>
    ''' <remarks> Nothing is not set. </remarks>
    ''' <value> The Open channels. </value>
    Public Overloads Property OpenChannels As String
        Get
            Return Me._OpenChannels
        End Get
        Protected Set(ByVal value As String)
            If Not String.Equals(value, Me.OpenChannels) Then
                Me._OpenChannels = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Applies the Open channels described by value. </summary>
    ''' <param name="value">   The scan list. </param>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function ApplyOpenChannels(ByVal value As String, ByVal timeout As TimeSpan) As String
        Me.WriteOpenChannels(value, timeout)
        Return Me.QueryOpenChannels()
    End Function

    ''' <summary> Gets the Open channels query command. </summary>
    ''' <value> The Open channels query command. </value>
    ''' <remarks> :ROUT:CLOS </remarks>
    Protected Overridable ReadOnly Property OpenChannelsQueryCommand As String

    ''' <summary> Queries Open channels. </summary>
    ''' <returns> The Open channels. </returns>
    Public Function QueryOpenChannels() As String
        Me.OpenChannels = Me.Query(Me.OpenChannels, Me.OpenChannelsQueryCommand)
        Return Me.OpenChannels
    End Function

    ''' <summary> Gets the open channels command format. </summary>
    ''' <value> The open channels command format. </value>
    ''' <remarks> :ROUT:OPEN </remarks>
    Protected Overridable ReadOnly Property OpenChannelsCommandFormat As String

    ''' <summary> Opens the specified channels in the list. </summary>
    ''' <param name="channelList"> List of channels. </param>
    ''' <param name="timeout">     The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function WriteOpenChannels(ByVal channelList As String, ByVal timeout As TimeSpan) As String
        Me.Session.Execute(String.Format(Me.OpenChannelsCommandFormat, channelList))
        If timeout > TimeSpan.Zero Then Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        ' set to nothing to indicate that the value is not known -- requires reading.
        Me.ClosedChannels = Nothing
        Me.ClosedChannel = Nothing
        Me.OpenChannels = channelList
        Return Me.OpenChannels
    End Function

#End Region

#Region " OPEN ALL CHANNELS "

    ''' <summary> Gets the open channels command. </summary>
    ''' <value> The open channels command. </value>
    Protected Overridable ReadOnly Property OpenChannelsCommand As String

    ''' <summary> Applies the open all command, wait for timeout and read back the closed channels. </summary>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function ApplyOpenAll(ByVal timeout As TimeSpan) As String
        Me.WriteOpenAll(timeout)
        Return Me.QueryOpenChannels
    End Function

    ''' <summary> Opens all channels. </summary>
    ''' <param name="timeout"> The timeout. </param>
    ''' <returns> A String. </returns>
    Public Function WriteOpenAll(ByVal timeout As TimeSpan) As String
        Me.Session.Execute(Me.OpenChannelsCommand)
        If timeout > TimeSpan.Zero Then Me.StatusSubsystem.AwaitOperationCompleted(timeout)
        ' set to nothing to indicate that the value is not known -- requires reading.
        Me.ClosedChannels = Nothing
        Me.ClosedChannel = Nothing
        Me.OpenChannels = Nothing
        Return Me.OpenChannels
    End Function

#End Region

#Region " CHANNEL PATERN = MEMORY SCANS "

    ''' <summary> Gets the recall channel pattern command format. </summary>
    ''' <value> The recall channel pattern command format. </value>
    Protected Overridable ReadOnly Property RecallChannelPatternCommandFormat As String

    ''' <summary> Recalls channel pattern from a memory location. </summary>
    ''' <param name="memoryLocation"> Specifies a memory location between 1 and 100. </param>
    ''' <param name="timeout">        The timeout. </param>
    Public Sub RecallChannelPattern(ByVal memoryLocation As Integer, ByVal timeout As TimeSpan)
        Me.Session.Execute(String.Format(Me.RecallChannelPatternCommandFormat, memoryLocation))
        If timeout > TimeSpan.Zero Then Me.StatusSubsystem.AwaitOperationCompleted(timeout)
    End Sub

    ''' <summary> Gets the save channel pattern command format. </summary>
    ''' <value> The save channel pattern command format. </value>
    Protected Overridable ReadOnly Property SaveChannelPatternCommandFormat As String

    ''' <summary> Saves existing channel pattern into a memory location. </summary>
    ''' <param name="memoryLocation"> Specifies a memory location between 1 and 100. </param>
    ''' <param name="timeout">        The timeout. </param>
    Public Sub SaveChannelPattern(ByVal memoryLocation As Integer, ByVal timeout As TimeSpan)
        Me.Session.Execute(String.Format(Me.SaveChannelPatternCommandFormat, memoryLocation))
        If timeout > TimeSpan.Zero Then Me.StatusSubsystem.AwaitOperationCompleted(timeout)
    End Sub

    ''' <summary> Saves a channel list to a memory item. </summary>
    ''' <param name="channelList"> List of channels. </param>
    ''' <returns> The memory location. </returns>
    Public Function SaveChannelPattern(ByVal channelList As String, ByVal memoryLocation As Integer, ByVal timeout As TimeSpan) As Integer
        If Not String.IsNullOrWhiteSpace(channelList) Then
            Me.WriteClosedChannels(channelList, timeout)
            Me.SaveChannelPattern(memoryLocation, timeout)
            Me.WriteOpenAll(timeout)
        End If
        Return memoryLocation
    End Function

    ''' <summary> Gets the one-based location of the first memory location of the default channel pattern set. </summary>
    ''' <value> The first memory location of the default channel pattern set. </value>
    Public ReadOnly Property FirstMemoryLocation As Integer

    ''' <summary> Gets the one-based location of the memory location of the default channel pattern set. </summary>
    ''' <value> The last automatic scan index. </value>
    Public ReadOnly Property LastMemoryLocation As Integer

    ''' <summary> Initializes the memory locations. </summary>
    Public Sub InitializeMemoryLocation()
        Me._FirstMemoryLocation = 0
        Me._LastMemoryLocation = 0
    End Sub

    ''' <summary>
    ''' Adds a channel list to the <see cref="LastMemoryLocation">+1: first available memory
    ''' location</see>.
    ''' </summary>
    ''' <param name="channelList"> List of channels. </param>
    ''' <param name="timeout">     The timeout. </param>
    ''' <returns> The new memory location. </returns>
    Public Function MemorizeChannelPattern(ByVal channelList As String, ByVal timeout As TimeSpan) As Integer
        If Not String.IsNullOrWhiteSpace(channelList) Then
            Me._LastMemoryLocation += 1
            If Me.LastMemoryLocation = 1 Then
                Me._FirstMemoryLocation = Me.LastMemoryLocation
            End If
            Return Me.SaveChannelPattern(channelList, Me.LastMemoryLocation, timeout)
        End If
        Return Me.LastMemoryLocation
    End Function

#End Region

#Region " SCAN LIST "

    ''' <summary> List of scans. </summary>
    Private _ScanList As String

    ''' <summary> Gets or sets the cached Scan List. </summary>
    ''' <value> A List of scans. </value>
    Public Overloads Property ScanList As String
        Get
            Return Me._ScanList
        End Get
        Protected Set(ByVal value As String)
            If String.IsNullOrWhiteSpace(value) Then value = ""
            If Not String.Equals(value, Me.ScanList, StringComparison.OrdinalIgnoreCase) OrElse
                Me._ScanList = value Then
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Scan List. </summary>
    ''' <param name="value"> The scan list. </param>
    ''' <returns> A List of scans. </returns>
    Public Function ApplyScanList(ByVal value As String) As String
        Me.WriteScanList(value)
        Return Me.QueryScanList()
    End Function

    ''' <summary> Gets the scan list command query. </summary>
    ''' <value> The scan list query command. </value>
    Protected Overridable ReadOnly Property ScanListQueryCommand As String

    ''' <summary> Queries the Scan List. Also sets the <see cref="ScanList">Route on</see> sentinel. </summary>
    ''' <returns> A List of scans. </returns>
    Public Function QueryScanList() As String
        Me.ScanList = Me.Query(Me.ScanList, Me.ScanListQueryCommand)
        Return Me.ScanList
    End Function

    ''' <summary> Gets the scan list command format. </summary>
    ''' <value> The scan list command format. </value>
    Protected Overridable ReadOnly Property ScanListCommandFormat As String

    ''' <summary> Writes the Scan List. Does not read back from the instrument. </summary>
    ''' <param name="value"> The scan list. </param>
    ''' <returns> A List of scans. </returns>
    Public Function WriteScanList(ByVal value As String) As String
        Me.Write(Me.ScanListCommandFormat, value)
        Me.ScanList = value
        Return Me.ScanList
    End Function

#End Region

#Region " SLOT CARD TYPE "

    ''' <summary> Gets the slot card type query command format. </summary>
    ''' <value> The slot card type query command format. </value>
    Protected Overridable ReadOnly Property SlotCardTypeQueryCommandFormat As String

    Private _SlotCardTypes As Dictionary(Of Integer, String)

    ''' <summary> Slot card type. </summary>
    ''' <param name="slotNumber"> The slot number. </param>
    ''' <returns> A String. </returns>
    Public Function SlotCardType(ByVal slotNumber As Integer) As String
        If Me._SlotCardTypes?.ContainsKey(slotNumber) Then
            Return Me._SlotCardTypes(slotNumber)
        Else
            Return ""
        End If
    End Function

    ''' <summary> Applies the card type. </summary>
    ''' <param name="cardNumber"> The card number. </param>
    ''' <param name="cardType">   Type of the card. </param>
    ''' <returns> A String. </returns>
    Public Function ApplySlotCardType(ByVal cardNumber As Integer, ByVal cardType As String) As String
        Me.WriteSlotCardType(cardNumber, cardType)
        Return Me.QuerySlotCardType(cardNumber)
    End Function

    ''' <summary> Queries the Slot Card Type. </summary>
    ''' <param name="slotNumber"> The slot number. </param>
    ''' <returns> A Slot Card Type. </returns>
    Public Function QuerySlotCardType(ByVal slotNumber As Integer) As String
        Dim value As String = ""
        If String.IsNullOrWhiteSpace(Me.SlotCardTypeQueryCommandFormat) Then
            value = ""
        Else
            value = Me.Query("", String.Format(Me.SlotCardTypeQueryCommandFormat, slotNumber))
        End If
        If _SlotCardTypes Is Nothing Then Me._SlotCardTypes = New Dictionary(Of Integer, String)
        If Me._SlotCardTypes.ContainsKey(slotNumber) Then
            Me._SlotCardTypes.Remove(slotNumber)
        End If
        If Not String.IsNullOrWhiteSpace(value) Then
            Me._SlotCardTypes.Add(slotNumber, value)
        End If
        Return value
    End Function

    ''' <summary> Gets the slot card type command format. </summary>
    ''' <value> The slot card type command format. </value>
    Protected Overridable ReadOnly Property SlotCardTypeCommandFormat As String

    ''' <summary> Writes a slot card type. </summary>
    ''' <param name="cardNumber"> The card number. </param>
    ''' <param name="cardType">   Type of the card. </param>
    ''' <returns> A String. </returns>
    Public Function WriteSlotCardType(ByVal cardNumber As Integer, ByVal cardType As String) As String
        If Not String.IsNullOrWhiteSpace(Me.SlotCardTypeCommandFormat) Then
            Me.Write("", String.Format(Me.SlotCardTypeCommandFormat, cardNumber, cardType))
        End If
        Return cardType
    End Function

#End Region

#Region " SLOT CARD SETTLING TIME "

    ''' <summary> Gets the slot card settling time query command format. </summary>
    ''' <value> The slot card settling time query command format. </value>
    Protected Overridable ReadOnly Property SlotCardSettlingTimeQueryCommandFormat As String

    Private _SlotCardSettlingTimes As Dictionary(Of Integer, TimeSpan)

    ''' <summary> Slot card settling time. </summary>
    ''' <param name="slotNumber"> The slot number. </param>
    ''' <returns> A TimeSpan. </returns>
    Public Function SlotCardSettlingTime(ByVal slotNumber As Integer) As TimeSpan
        Dim ts As TimeSpan = TimeSpan.Zero
        If Me._SlotCardSettlingTimes?.ContainsKey(slotNumber) Then
            ts = Me._SlotCardSettlingTimes(slotNumber)
        End If
        Return ts
    End Function

    ''' <summary> Applies the slot card settling time. </summary>
    ''' <param name="cardNumber">   The card number. </param>
    ''' <param name="settlingTime"> The settling time. </param>
    ''' <returns> A TimeSpan. </returns>
    Public Function ApplySlotCardSettlingTime(ByVal cardNumber As Integer, ByVal settlingTime As TimeSpan) As TimeSpan
        Me.WriteSlotCardSettlingTime(cardNumber, settlingTime)
        Return Me.QuerySlotCardSettlingTime(cardNumber)
    End Function

    ''' <summary> Queries the Slot Card settling time. </summary>
    ''' <param name="slotNumber"> The slot number. </param>
    ''' <returns> A Slot Card settling time. </returns>
    Public Function QuerySlotCardSettlingTime(ByVal slotNumber As Integer) As TimeSpan
        Dim ts As TimeSpan = TimeSpan.Zero
        If Not String.IsNullOrWhiteSpace(Me.SlotCardSettlingTimeQueryCommandFormat) Then
            Dim value As Double? = Me.Query(New Double?, String.Format(Me.SlotCardSettlingTimeQueryCommandFormat, slotNumber))
            If value.HasValue Then
                ts = TimeSpan.FromTicks(CLng(TimeSpan.TicksPerSecond * value.Value))
            Else
                ts = TimeSpan.Zero
            End If
        End If
        If Me._SlotCardSettlingTimes Is Nothing Then Me._SlotCardSettlingTimes = New Dictionary(Of Integer, TimeSpan)
        If Me._SlotCardSettlingTimes.ContainsKey(slotNumber) Then
            Me._SlotCardSettlingTimes.Remove(slotNumber)
        End If
        If ts <> TimeSpan.Zero Then
            Me._SlotCardSettlingTimes.Add(slotNumber, ts)
        End If
        Return ts
    End Function

    ''' <summary> Gets the slot card settling time command format. </summary>
    ''' <value> The slot card settling time command format. </value>
    Protected Overridable ReadOnly Property SlotCardSettlingTimeCommandFormat As String

    ''' <summary> Writes a slot card settling time. </summary>
    ''' <param name="cardNumber">   The card number. </param>
    ''' <param name="settlingTime"> The settling time. </param>
    ''' <returns> A TimeSpan. </returns>
    Public Function WriteSlotCardSettlingTime(ByVal cardNumber As Integer, ByVal settlingTime As TimeSpan) As TimeSpan
        If Not String.IsNullOrWhiteSpace(Me.SlotCardSettlingTimeCommandFormat) Then
            Me.Write(CDbl(settlingTime.TotalSeconds),
                     String.Format(Me.SlotCardSettlingTimeCommandFormat, cardNumber, settlingTime.TotalSeconds))
        End If
        Return settlingTime
    End Function

#End Region

End Class
