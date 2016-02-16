Imports System.ComponentModel
''' <summary> Base class manager of VISA resource names. </summary>
''' <remarks> David, 11/21/2015. </remarks>
''' <license>
''' (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="11/21/2015" by="David" revision=""> Created. </history>
Public NotInheritable Class ResourceNamesManager

#Region " CONSTRUCTORS "

    ''' <summary>
    ''' Constructor that prevents a default instance of this class from being created.
    ''' </summary>
    ''' <remarks> David, 11/23/2015. </remarks>
    Private Sub New()
        MyBase.New()
    End Sub

#End Region

#Region " SEARCH PATTERNS "

    Public Const GpibResourceBaseName As String = "GPIB"
    Public Const GpibVxiResourceBaseName As String = "GPIBVxi"
    Public Const SerialResourceBaseName As String = "ASRL"
    Public Const TcpIPResourceBaseName As String = "TCPIP"
    Public Const UsbResourceBaseName As String = "USB"
    Public Const PxiResourceBaseName As String = "Pxi"
    Public Const VxiResourceBaseName As String = "Vxi"

    Public Const InterfaceBaseName As String = "INTFC"
    Public Const InstrumentBaseName As String = "INSTR"
    Public Const BackplaneBaseName As String = "BACKPLANE"
    Public Const RawBaseName As String = "RAW"

    ''' <summary> A pattern specifying all resources search. </summary>
    Public Const AllResourcesFilter As String = "?*"

    ''' <summary> A pattern specifying the Gpib, USB or TCPIP search. </summary>
    Public Const GpibUsbTcpIPFilter As String = "(GPIB|USB|TCPIP)?*"

    ''' <summary> A pattern specifying the Gpib, USB or TCPIP instrument search. </summary>
    Public Const GpibUsbTcpIPInstrumentFilter As String = GpibUsbTcpIPFilter & InstrumentBaseName

    Public Const BackplaneFilterFormat As String = "{0}?*" & BackplaneBaseName
    Public Const RawFilterFormat As String = "{0}?*" & RawBaseName
    Public Const InterfaceFilterFormat As String = "{0}?*" & InterfaceBaseName
    Public Const InstrumentFilterFormat As String = "{0}?*" & InstrumentBaseName

    Public Const InstrumentBoardFilterFormat As String = "{0}{1}?*" & InstrumentBaseName
    Public Const InterfaceResourceFormat As String = "{0}{1}::" & InterfaceBaseName

    ''' <summary> Parse address. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="resourceName"> The name of the resource. </param>
    ''' <returns> A String. </returns>
    Public Shared Function ParseAddress(ByVal resourceName As String) As String
        Dim address As String = ""
        If Not String.IsNullOrWhiteSpace(resourceName) Then
            Dim addressLocation As Int32 = resourceName.IndexOf("::", StringComparison.OrdinalIgnoreCase) + 2
            Dim addressWidth As Int32 = resourceName.LastIndexOf("::", StringComparison.OrdinalIgnoreCase) - addressLocation
            address = resourceName.Substring(addressLocation, addressWidth)
        End If
        Return address
    End Function

    ''' <summary> Parse gpib address. </summary>
    ''' <remarks> David, 11/23/2015. </remarks>
    ''' <param name="resourceName"> The name of the resource. </param>
    ''' <returns> An Integer. </returns>
    Public Shared Function ParseGpibAddress(ByVal resourceName As String) As Integer
        Dim value As Integer = 0
        Dim address As String = ResourceNamesManager.ParseAddress(resourceName)
        If Integer.TryParse(address, value) Then
        End If
        Return value
    End Function

    ''' <summary> Parse interface number. </summary>
    ''' <remarks> David, 1/26/2016. </remarks>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <returns> An Integer. </returns>
    Public Shared Function ParseInterfaceNumber(ByVal resourceName As String) As Integer
        If String.IsNullOrWhiteSpace(resourceName) Then Throw New ArgumentNullException(NameOf(resourceName))
        Dim result As Integer = 0
        Dim interfaceType As HardwareInterfaceType = ResourceNamesManager.ParseHardwareInterfaceType(resourceName)
        Dim baseName As String = ResourceNamesManager.InterfaceResourceBaseName(interfaceType)
        Dim parts As String() = resourceName.Split(":"c)
        If parts.Count <= 0 OrElse parts(0).Length <= baseName.Length OrElse Not Integer.TryParse(parts(0).Substring(baseName.Length), result) Then
            result = 0
        End If
        Return result
    End Function


    ''' <summary> Parse resource type. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <returns> A HardwareInterfaceType. </returns>
    Public Shared Function ParseResourceType(ByVal resourceName As String) As ResourceType
        Dim result As ResourceType = ResourceType.None
        If Not String.IsNullOrWhiteSpace(resourceName) Then
            For Each v As ResourceType In [Enum].GetValues(GetType(ResourceType))
                If v <> ResourceType.None AndAlso
                    resourceName.EndsWith(ResourceNamesManager.ResourceTypeBaseName(v), StringComparison.OrdinalIgnoreCase) Then
                    result = v
                    Exit For
                End If
            Next
        End If
        Return result
    End Function

    ''' <summary> Resource type base name. </summary>
    ''' <remarks> David, 1/27/2016. </remarks>
    ''' <param name="resourceType"> Type of the resource. </param>
    ''' <returns> A String. </returns>
    Public Shared Function ResourceTypeBaseName(ByVal resourceType As ResourceType) As String
        Dim result As String = ""
        Select Case resourceType
            Case ResourceType.None
            Case ResourceType.Instrument
                result = ResourceNamesManager.InstrumentBaseName
            Case ResourceType.Interface
                result = ResourceNamesManager.InterfaceBaseName
            Case ResourceType.Backplane
                result = ResourceNamesManager.BackplaneBaseName
        End Select
        Return result
    End Function



    ''' <summary> Parse hardware interface type. </summary>
    ''' <remarks> David, 1/25/2016. </remarks>
    ''' <param name="resourceName"> Name of the resource. </param>
    ''' <returns> A HardwareInterfaceType. </returns>
    Public Shared Function ParseHardwareInterfaceType(ByVal resourceName As String) As HardwareInterfaceType
        Dim result As HardwareInterfaceType = HardwareInterfaceType.Custom
        If Not String.IsNullOrWhiteSpace(resourceName) Then
            For Each v As HardwareInterfaceType In [Enum].GetValues(GetType(HardwareInterfaceType))
                If v <> HardwareInterfaceType.Custom AndAlso
                    resourceName.StartsWith(ResourceNamesManager.InterfaceResourceBaseName(v), StringComparison.OrdinalIgnoreCase) Then
                    result = v
                    Exit For
                End If
            Next
        End If
        Return result
    End Function

    ''' <summary> Returns the interface resource base name. </summary>
    ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    ''' illegal values. </exception>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> The interface base name. </returns>
    Public Shared Function InterfaceResourceBaseName(ByVal interfaceType As HardwareInterfaceType) As String
        Select Case interfaceType
            Case HardwareInterfaceType.Gpib
                Return GpibResourceBaseName
            Case HardwareInterfaceType.GpibVxi
                Return GpibVxiResourceBaseName
            Case HardwareInterfaceType.Pxi
                Return PxiResourceBaseName
            Case HardwareInterfaceType.Serial
                Return SerialResourceBaseName
            Case HardwareInterfaceType.Tcpip
                Return TcpIPResourceBaseName
            Case HardwareInterfaceType.Usb
                Return UsbResourceBaseName
            Case HardwareInterfaceType.Vxi
                Return VxiResourceBaseName
            Case Else
                Throw New ArgumentException("Unhandled case", "interfaceType")
        End Select
    End Function

    ''' <summary> Returns the Interface resource name. </summary>
    ''' <param name="interfaceName"> The name of the interface. </param>
    ''' <param name="boardNumber">   The board number. </param>
    ''' <returns> The Interface resource name, e.g., 'GPIB?*INTFC'. </returns>
    Public Shared Function BuildInterfaceResourceName(ByVal interfaceName As String, ByVal boardNumber As Integer) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, ResourceNamesManager.InterfaceResourceFormat,
                             interfaceName, boardNumber)
    End Function

    ''' <summary> Returns the Interface resource name. </summary>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <param name="boardNumber">   The board number. </param>
    ''' <returns> The Interface resource name, e.g., 'GPIB?*INTFC'. </returns>
    Public Shared Function BuildInterfaceResourceName(ByVal interfaceType As HardwareInterfaceType, ByVal boardNumber As Integer) As String
        Return ResourceNamesManager.BuildInterfaceResourceName(ResourceNamesManager.InterfaceResourceBaseName(interfaceType), boardNumber)
    End Function

    ''' <summary> Returns the Interface search pattern. </summary>
    ''' <returns> The Interface search pattern '?*INTFC'. </returns>
    Public Shared Function BuildInterfaceFilter() As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, ResourceNamesManager.InterfaceFilterFormat, "")
    End Function

    ''' <summary> Returns the Interface search pattern. </summary>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> The Interface search pattern, e.g., 'GPIB?*INTFC'. </returns>
    Public Shared Function BuildInterfaceFilter(ByVal interfaceType As HardwareInterfaceType) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, ResourceNamesManager.InterfaceFilterFormat,
                             ResourceNamesManager.InterfaceResourceBaseName(interfaceType))
    End Function

    ''' <summary> Returns the Instrument search pattern. </summary>
    ''' <returns> The Instrument search pattern, e.g., '?*INSTR'. </returns>
    Public Shared Function BuildInstrumentFilter() As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, ResourceNamesManager.InstrumentFilterFormat, "")
    End Function

    ''' <summary> Returns the Instrument search pattern. </summary>
    ''' <param name="interfaceType"> Type of the interface. </param>
    ''' <returns> The Instrument search pattern, e.g., 'GPIB?*INSTR'. </returns>
    Public Shared Function BuildInstrumentFilter(ByVal interfaceType As HardwareInterfaceType) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, ResourceNamesManager.InstrumentFilterFormat,
                             ResourceNamesManager.InterfaceResourceBaseName(interfaceType))
    End Function

    ''' <summary> Returns the Instrument search pattern. </summary>
    ''' <param name="interface1"> Type of the interface. </param>
    ''' <returns> The Instrument search pattern, e.g., '(GPIB|USB)?*INSTR'. </returns>
    Public Shared Function BuildInstrumentFilter(ByVal interface1 As HardwareInterfaceType,
                                                 ByVal interface2 As HardwareInterfaceType) As String
        Return $"({ResourceNamesManager.InterfaceResourceBaseName(interface1)}|{ResourceNamesManager.InterfaceResourceBaseName(interface2)})?*{InstrumentBaseName}"
    End Function
    ''' <summary> Returns the Instrument search pattern. </summary>
    ''' <param name="interface1"> Type of the interface. </param>
    ''' <returns> The Instrument search pattern, e.g., '(GPIB|USB)?*INSTR'. </returns>
    Public Shared Function BuildInstrumentFilter(ByVal interface1 As HardwareInterfaceType,
                                                 ByVal interface2 As HardwareInterfaceType,
                                                 ByVal interface3 As HardwareInterfaceType) As String
        Return $"({ResourceNamesManager.InterfaceResourceBaseName(interface1)}|{ResourceNamesManager.InterfaceResourceBaseName(interface2)}|{ResourceNamesManager.InterfaceResourceBaseName(interface3)})?*{InstrumentBaseName}"
    End Function


    ''' <summary> Returns the Instrument search pattern. </summary>
    ''' <param name="interfaceType">   Type of the interface. </param>
    ''' <param name="interfaceNumber"> The interface number (e.g., board or port number). </param>
    ''' <returns> The Instrument search pattern, e.g., 'GPIB0?*INSTR'. </returns>
    Public Shared Function BuildInstrumentFilter(ByVal interfaceType As HardwareInterfaceType, ByVal interfaceNumber As Integer) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, ResourceNamesManager.InstrumentBoardFilterFormat,
                             ResourceNamesManager.InterfaceResourceBaseName(interfaceType), interfaceNumber)
    End Function

#End Region

#Region " RESOURCE NAMES "

    ''' <summary> The Gpib instrument resource format. </summary>
    Private Const _GpibInstrumentResourceFormat As String = "GPIB{0}::{1}::INSTR"

    ''' <summary> Builds Gpib instrument resource. </summary>
    ''' <param name="boardNumber"> The board number. </param>
    ''' <param name="address">     The address. </param>
    ''' <returns> The resource name. </returns>
    Public Shared Function BuildGpibInstrumentResource(ByVal boardNumber As Integer, ByVal address As Integer) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, ResourceNamesManager._GpibInstrumentResourceFormat,
                             boardNumber, address)
    End Function

    ''' <summary> Builds Gpib instrument resource. </summary>
    ''' <param name="boardNumber"> The board number. </param>
    ''' <param name="address">     The address. </param>
    ''' <returns> The resource name. </returns>
    Public Shared Function BuildGpibInstrumentResource(ByVal boardNumber As String, ByVal address As String) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, ResourceNamesManager._GpibInstrumentResourceFormat,
                             boardNumber, address)
    End Function

    ''' <summary> The USB instrument resource format. </summary>
    Private Const _UsbInstrumentResourceFormat As String = "USB{0}::0x{1:X}::0x{2:X}::{3}::INSTR"

    ''' <summary> Builds Gpib instrument resource. </summary>
    ''' <param name="boardNumber"> The board number. </param>
    ''' <returns> The resource name. </returns>
    Public Shared Function BuildUsbInstrumentResource(ByVal boardNumber As Integer, ByVal manufacturerId As Integer,
                                                      ByVal modelNumber As Integer, ByVal serialNumber As Integer) As String
        Return String.Format(Globalization.CultureInfo.CurrentCulture, ResourceNamesManager._UsbInstrumentResourceFormat,
                             boardNumber, manufacturerId, modelNumber, serialNumber)
    End Function

#End Region

End Class

''' <summary> Values that represent resource types. </summary>
''' <remarks> David, 1/27/2016. </remarks>
Public Enum ResourceType
    <Description("Not specified")> None = 0
    <Description("Instrument")> Instrument
    <Description("Interface")> [Interface]
    <Description("Backplane")> Backplane
End Enum

''' <summary> Values that represent hardware interface types. </summary>
''' <remarks> David, 11/21/2015. </remarks>
Public Enum HardwareInterfaceType
    <Description("Custom")> Custom = 0
    ''' <summary> An enum constant representing the gpib interface. </summary>
    <Description("GPIB")> Gpib = 1
    ''' <summary> An enum constant representing the vxi option. </summary>
    <Description("VXI")> Vxi = 2
    ''' <summary> An enum constant representing the gpib vxi option. </summary>
    <Description("GPIB VXI")> GpibVxi = 3
    ''' <summary> An enum constant representing the serial option. </summary>
    <Description("Serial")> Serial = 4
    ''' <summary> An enum constant representing the pxi option. </summary>
    <Description("PXI")> Pxi = 5
    ''' <summary> An enum constant representing the TCP/IP option. </summary>
    <Description("TCPIP")> Tcpip = 6
    ''' <summary> An enum constant representing the USB option. </summary>
    <Description("USB")> Usb = 7
End Enum
