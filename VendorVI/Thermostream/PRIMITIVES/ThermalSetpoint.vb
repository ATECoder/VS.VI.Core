''' <summary> Thermal setpoint. </summary>
''' <license> (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="3/26/2015" by="David"> > Created. </history>
Public Class ThermalSetpoint

    ''' <summary> Default constructor. </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary> Constructor. </summary>
    ''' <param name="ordinalNumber"> The ordinal number. </param>
    Public Sub New(ByVal ordinalNumber As Integer)
        Me.New()
        Me._OrdinalNumber = ordinalNumber
    End Sub

    ''' <summary> Gets or sets the profile element. </summary>
    ''' <value> The setpoint number. </value>
    Public Property OrdinalNumber As Integer

    ''' <summary> Gets or sets the temperature. </summary>
    ''' <value> The temperature. </value>
    Public Property Temperature As Double

    ''' <summary> Gets or sets the ramp rate in degrees per second. </summary>
    ''' <value> The ramp rate. </value>
    Public Property RampRate As Double

    ''' <summary> Gets or sets the soak seconds. </summary>
    ''' <value> The soak seconds. </value>
    Public Property SoakSeconds As Integer

    ''' <summary> Gets or sets the window. </summary>
    ''' <value> The window. </value>
    Public Property Window As Double

End Class

''' <summary> Thermal profile collection. </summary>
''' <license> (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
''' SOFTWARE.</para> </license>
''' <history date="3/26/2015" by="David"> > Created. </history>
Public Class ThermalProfileCollection
    Inherits ObjectModel.KeyedCollection(Of Integer, ThermalSetpoint)

    ''' <summary> Gets key for item. </summary>
    ''' <param name="item"> The item. </param>
    ''' <returns> The key for item. </returns>
    Protected Overrides Function GetKeyForItem(item As ThermalSetpoint) As Integer
        If item Is Nothing Then Throw New ArgumentNullException(NameOf(item))
        Return item.OrdinalNumber
    End Function

    ''' <summary> Adds a new setpoint. </summary>
    ''' <param name="ordinalNumber"> The ordinal number. </param>
    ''' <returns> A ThermalSetpoint. </returns>
    Public Function AddNewSetpoint(ByVal ordinalNumber As Integer) As ThermalSetpoint
        Dim setPoint As ThermalSetpoint = New ThermalSetpoint(ordinalNumber)
        Me.Add(setPoint)
        Return setPoint
    End Function

End Class