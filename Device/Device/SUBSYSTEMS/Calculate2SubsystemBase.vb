Imports isr.Core.Pith
''' <summary> Defines the CALC2 SCPI subsystem. </summary>
''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
''' Licensed under The MIT License. </para><para>
''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
''' </para> </license>
''' <history date="11/05/2013" by="David" revision="">            Created based on SCPI 5.1 library. </history>
''' <history date="03/25/2008" by="David" revision="5.0.3004.x">  Port to new SCPI library. </history>
Public MustInherit Class Calculate2SubsystemBase
    Inherits SubsystemPlusStatusBase

#Region " CONSTRUCTION + CLEANUP "

    ''' <summary> Initializes a new instance of the <see cref="Calculate2SubsystemBase" /> class. </summary>
    ''' <param name="statusSubsystem "> A reference to a <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    Protected Sub New(ByVal statusSubsystem As VI.StatusSubsystemBase)
        MyBase.New(statusSubsystem)
    End Sub

    ''' <summary>
    ''' Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" />
    ''' and its child controls and optionally releases the managed resources.
    ''' </summary>
    ''' <param name="disposing"> <c>True</c> to release both managed and unmanaged resources;
    '''                          <c>False</c> to release only unmanaged resources when called from the
    '''                          runtime finalize. </param>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If Not Me.IsDisposed AndAlso disposing Then
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#End Region

#Region " CALC 2 "

    ''' <summary>
    ''' Return the average of the buffer contents.
    ''' </summary>
    Public Function CalculateBufferAverage() As Double

        ' select average
        Me.Session.WriteLine("CALC2:FORM:MEAN")

        ' turn status on.
        Me.Session.WriteLine("CALC2:STAT:ON")

        ' do the calculation.
        Me.Session.WriteLine("CALC2:IMM")

        ' get the result
        Return Me.Session.Query(0.0F, ":CALC2:DATA")

    End Function

#End Region

#Region " LIMIT 1 "

#Region " LIMIT1 ENABLED "

    ''' <summary> Limit1 enabled. </summary>
    Private _Limit1Enabled As Boolean?

    ''' <summary> Gets or sets the cached Limit1 Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Limit1 Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Limit1Enabled As Boolean?
        Get
            Return Me._Limit1Enabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Limit1Enabled, value) Then
                Me._Limit1Enabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit1 Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyLimit1Enabled(ByVal value As Boolean) As Boolean?
        Me.WriteLimit1Enabled(value)
        Return Me.QueryLimit1Enabled()
    End Function

    ''' <summary> Gets or sets the Limit1 enabled query command. </summary>
    ''' <value> The Limit1 enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:FRES:LIM1:STAT?" </remarks>
    Protected Overridable ReadOnly Property Limit1EnabledQueryCommand As String

    ''' <summary> Queries the Limit1 Enabled sentinel. Also sets the
    ''' <see cref="Limit1Enabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryLimit1Enabled() As Boolean?
        Me.Limit1Enabled = Me.Query(Me.Limit1Enabled, Me.Limit1EnabledQueryCommand)
        Return Me.Limit1Enabled
    End Function

    ''' <summary> Gets or sets the Limit1 enabled command Format. </summary>
    ''' <value> The Limit1 enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:FRES:LIM1:STAT {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property Limit1EnabledCommandFormat As String

    ''' <summary> Writes the Limit1 Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteLimit1Enabled(ByVal value As Boolean) As Boolean?
        Me.Limit1Enabled = Me.Write(value, Me.Limit1EnabledCommandFormat)
        Return Me.Limit1Enabled
    End Function

#End Region

#Region " LIMIT1 LOWER LEVEL "

    ''' <summary> The Limit1 Lower Level. </summary>
    Private _Limit1LowerLevel As Double?

    ''' <summary> Gets or sets the cached Limit1 Lower Level. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Limit1LowerLevel As Double?
        Get
            Return Me._Limit1LowerLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Limit1LowerLevel, value) Then
                Me._Limit1LowerLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit1 Lower Level. </summary>
    ''' <param name="value"> The Limit1 Lower Level. </param>
    ''' <returns> The Limit1 Lower Level. </returns>
    Public Function ApplyLimit1LowerLevel(ByVal value As Double) As Double?
        Me.WriteLimit1LowerLevel(value)
        Return Me.QueryLimit1LowerLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Lower Level query command. </summary>
    ''' <value> The Limit1 Lower Level query command. </value>
    Protected Overridable ReadOnly Property Limit1LowerLevelQueryCommand As String

    ''' <summary> Queries The Limit1 Lower Level. </summary>
    ''' <returns> The Limit1 Lower Level or none if unknown. </returns>
    Public Function QueryLimit1LowerLevel() As Double?
        Me.Limit1LowerLevel = Me.Query(Me.Limit1LowerLevel, Me.Limit1LowerLevelQueryCommand)
        Return Me.Limit1LowerLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Lower Level command format. </summary>
    ''' <value> The Limit1 Lower Level command format. </value>
    Protected Overridable ReadOnly Property Limit1LowerLevelCommandFormat As String

    ''' <summary> Writes The Limit1 Lower Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Limit1 Lower Level. </remarks>
    ''' <param name="value"> The Limit1 Lower Level. </param>
    ''' <returns> The Limit1 Lower Level. </returns>
    Public Function WriteLimit1LowerLevel(ByVal value As Double) As Double?
        Me.Limit1LowerLevel = Me.Write(value, Me.Limit1LowerLevelCommandFormat)
        Return Me.Limit1LowerLevel
    End Function

#End Region

#Region " LIMIT1 UPPER LEVEL "

    ''' <summary> The Limit1 Upper Level. </summary>
    Private _Limit1UpperLevel As Double?

    ''' <summary> Gets or sets the cached Limit1 Upper Level. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Limit1UpperLevel As Double?
        Get
            Return Me._Limit1UpperLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Limit1UpperLevel, value) Then
                Me._Limit1UpperLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit1 Upper Level. </summary>
    ''' <param name="value"> The Limit1 Upper Level. </param>
    ''' <returns> The Limit1 Upper Level. </returns>
    Public Function ApplyLimit1UpperLevel(ByVal value As Double) As Double?
        Me.WriteLimit1UpperLevel(value)
        Return Me.QueryLimit1UpperLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Upper Level query command. </summary>
    ''' <value> The Limit1 Upper Level query command. </value>
    Protected Overridable ReadOnly Property Limit1UpperLevelQueryCommand As String

    ''' <summary> Queries The Limit1 Upper Level. </summary>
    ''' <returns> The Limit1 Upper Level or none if unknown. </returns>
    Public Function QueryLimit1UpperLevel() As Double?
        Me.Limit1UpperLevel = Me.Query(Me.Limit1UpperLevel, Me.Limit1UpperLevelQueryCommand)
        Return Me.Limit1UpperLevel
    End Function

    ''' <summary> Gets or sets The Limit1 Upper Level command format. </summary>
    ''' <value> The Limit1 Upper Level command format. </value>
    Protected Overridable ReadOnly Property Limit1UpperLevelCommandFormat As String

    ''' <summary> Writes The Limit1 Upper Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Limit1 Upper Level. </remarks>
    ''' <param name="value"> The Limit1 Upper Level. </param>
    ''' <returns> The Limit1 Upper Level. </returns>
    Public Function WriteLimit1UpperLevel(ByVal value As Double) As Double?
        Me.Limit1UpperLevel = Me.Write(value, Me.Limit1UpperLevelCommandFormat)
        Return Me.Limit1UpperLevel
    End Function

#End Region

#Region " LIMIT1 AUTO CLEAR "

    ''' <summary> Limit1 Auto Clear. </summary>
    Private _Limit1AutoClear As Boolean?

    ''' <summary> Gets or sets the cached Limit1 Auto Clear sentinel. </summary>
    ''' <value> <c>null</c> if Limit1 Auto Clear is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Limit1AutoClear As Boolean?
        Get
            Return Me._Limit1AutoClear
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Limit1AutoClear, value) Then
                Me._Limit1AutoClear = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit1 Auto Clear sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function ApplyLimit1AutoClear(ByVal value As Boolean) As Boolean?
        Me.WriteLimit1AutoClear(value)
        Return Me.QueryLimit1AutoClear()
    End Function

    ''' <summary> Gets or sets the Limit1 Auto Clear query command. </summary>
    ''' <value> The Limit1 Auto Clear query command. </value>
    ''' <remarks> SCPI: ":CALC2:FRES:LIM1:STAT?" </remarks>
    Protected Overridable ReadOnly Property Limit1AutoClearQueryCommand As String

    ''' <summary> Queries the Limit1 Auto Clear sentinel. Also sets the
    ''' <see cref="Limit1AutoClear">AutoClear</see> sentinel. </summary>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function QueryLimit1AutoClear() As Boolean?
        Me.Limit1AutoClear = Me.Query(Me.Limit1AutoClear, Me.Limit1AutoClearQueryCommand)
        Return Me.Limit1AutoClear
    End Function

    ''' <summary> Gets or sets the Limit1 Auto Clear command Format. </summary>
    ''' <value> The Limit1 Auto Clear query command. </value>
    ''' <remarks> SCPI: ":CALC2:FRES:LIM1:STAT {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property Limit1AutoClearCommandFormat As String

    ''' <summary> Writes the Limit1 Auto Clear sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is Auto Clear. </param>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function WriteLimit1AutoClear(ByVal value As Boolean) As Boolean?
        Me.Limit1AutoClear = Me.Write(value, Me.Limit1AutoClearCommandFormat)
        Return Me.Limit1AutoClear
    End Function

#End Region

#End Region

#Region " LIMIT 2 "

#Region " LIMIT2 ENABLED "

    ''' <summary> Limit2 enabled. </summary>
    Private _Limit2Enabled As Boolean?

    ''' <summary> Gets or sets the cached Limit2 Enabled sentinel. </summary>
    ''' <value> <c>null</c> if Limit2 Enabled is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Limit2Enabled As Boolean?
        Get
            Return Me._Limit2Enabled
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Limit2Enabled, value) Then
                Me._Limit2Enabled = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit2 Enabled sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function ApplyLimit2Enabled(ByVal value As Boolean) As Boolean?
        Me.WriteLimit2Enabled(value)
        Return Me.QueryLimit2Enabled()
    End Function

    ''' <summary> Gets or sets the Limit2 enabled query command. </summary>
    ''' <value> The Limit2 enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:FRES:LIM2:STAT?" </remarks>
    Protected Overridable ReadOnly Property Limit2EnabledQueryCommand As String

    ''' <summary> Queries the Limit2 Enabled sentinel. Also sets the
    ''' <see cref="Limit2Enabled">Enabled</see> sentinel. </summary>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function QueryLimit2Enabled() As Boolean?
        Me.Limit2Enabled = Me.Query(Me.Limit2Enabled, Me.Limit2EnabledQueryCommand)
        Return Me.Limit2Enabled
    End Function

    ''' <summary> Gets or sets the Limit2 enabled command Format. </summary>
    ''' <value> The Limit2 enabled query command. </value>
    ''' <remarks> SCPI: ":CALC2:FRES:LIM2:STAT {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property Limit2EnabledCommandFormat As String

    ''' <summary> Writes the Limit2 Enabled sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is enabled. </param>
    ''' <returns> <c>True</c> if enabled; otherwise <c>False</c>. </returns>
    Public Function WriteLimit2Enabled(ByVal value As Boolean) As Boolean?
        Me.Limit2Enabled = Me.Write(value, Me.Limit2EnabledCommandFormat)
        Return Me.Limit2Enabled
    End Function

#End Region

#Region " LIMIT2 LOWER LEVEL "

    ''' <summary> The Limit2 Lower Level. </summary>
    Private _Limit2LowerLevel As Double?

    ''' <summary> Gets or sets the cached Limit2 Lower Level. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Limit2LowerLevel As Double?
        Get
            Return Me._Limit2LowerLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Limit2LowerLevel, value) Then
                Me._Limit2LowerLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit2 Lower Level. </summary>
    ''' <param name="value"> The Limit2 Lower Level. </param>
    ''' <returns> The Limit2 Lower Level. </returns>
    Public Function ApplyLimit2LowerLevel(ByVal value As Double) As Double?
        Me.WriteLimit2LowerLevel(value)
        Return Me.QueryLimit2LowerLevel
    End Function

    ''' <summary> Gets or sets The Limit2 Lower Level query command. </summary>
    ''' <value> The Limit2 Lower Level query command. </value>
    Protected Overridable ReadOnly Property Limit2LowerLevelQueryCommand As String

    ''' <summary> Queries The Limit2 Lower Level. </summary>
    ''' <returns> The Limit2 Lower Level or none if unknown. </returns>
    Public Function QueryLimit2LowerLevel() As Double?
        Me.Limit2LowerLevel = Me.Query(Me.Limit2LowerLevel, Me.Limit2LowerLevelQueryCommand)
        Return Me.Limit2LowerLevel
    End Function

    ''' <summary> Gets or sets The Limit2 Lower Level command format. </summary>
    ''' <value> The Limit2 Lower Level command format. </value>
    Protected Overridable ReadOnly Property Limit2LowerLevelCommandFormat As String

    ''' <summary> Writes The Limit2 Lower Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Limit2 Lower Level. </remarks>
    ''' <param name="value"> The Limit2 Lower Level. </param>
    ''' <returns> The Limit2 Lower Level. </returns>
    Public Function WriteLimit2LowerLevel(ByVal value As Double) As Double?
        Me.Limit2LowerLevel = Me.Write(value, Me.Limit2LowerLevelCommandFormat)
        Return Me.Limit2LowerLevel
    End Function

#End Region

#Region " LIMIT2 UPPER LEVEL "

    ''' <summary> The Limit2 Upper Level. </summary>
    Private _Limit2UpperLevel As Double?

    ''' <summary> Gets or sets the cached Limit2 Upper Level. Set to
    ''' <see cref="VI.Pith.Scpi.Syntax.Infinity">infinity</see> to set to maximum or to
    ''' <see cref="VI.Pith.Scpi.Syntax.NegativeInfinity">negative infinity</see> for minimum. </summary>
    ''' <value> <c>null</c> if value is not known. </value>
    Public Overloads Property Limit2UpperLevel As Double?
        Get
            Return Me._Limit2UpperLevel
        End Get
        Protected Set(ByVal value As Double?)
            If Not Nullable.Equals(Me.Limit2UpperLevel, value) Then
                Me._Limit2UpperLevel = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit2 Upper Level. </summary>
    ''' <param name="value"> The Limit2 Upper Level. </param>
    ''' <returns> The Limit2 Upper Level. </returns>
    Public Function ApplyLimit2UpperLevel(ByVal value As Double) As Double?
        Me.WriteLimit2UpperLevel(value)
        Return Me.QueryLimit2UpperLevel
    End Function

    ''' <summary> Gets or sets The Limit2 Upper Level query command. </summary>
    ''' <value> The Limit2 Upper Level query command. </value>
    Protected Overridable ReadOnly Property Limit2UpperLevelQueryCommand As String

    ''' <summary> Queries The Limit2 Upper Level. </summary>
    ''' <returns> The Limit2 Upper Level or none if unknown. </returns>
    Public Function QueryLimit2UpperLevel() As Double?
        Me.Limit2UpperLevel = Me.Query(Me.Limit2UpperLevel, Me.Limit2UpperLevelQueryCommand)
        Return Me.Limit2UpperLevel
    End Function

    ''' <summary> Gets or sets The Limit2 Upper Level command format. </summary>
    ''' <value> The Limit2 Upper Level command format. </value>
    Protected Overridable ReadOnly Property Limit2UpperLevelCommandFormat As String

    ''' <summary> Writes The Limit2 Upper Level without reading back the value from the device. </summary>
    ''' <remarks> This command sets The Limit2 Upper Level. </remarks>
    ''' <param name="value"> The Limit2 Upper Level. </param>
    ''' <returns> The Limit2 Upper Level. </returns>
    Public Function WriteLimit2UpperLevel(ByVal value As Double) As Double?
        Me.Limit2UpperLevel = Me.Write(value, Me.Limit2UpperLevelCommandFormat)
        Return Me.Limit2UpperLevel
    End Function

#End Region

#Region " LIMIT2 AUTO CLEAR "

    ''' <summary> Limit2 Auto Clear. </summary>
    Private _Limit2AutoClear As Boolean?

    ''' <summary> Gets or sets the cached Limit2 Auto Clear sentinel. </summary>
    ''' <value> <c>null</c> if Limit2 Auto Clear is not known; <c>True</c> if output is on; otherwise,
    ''' <c>False</c>. </value>
    Public Property Limit2AutoClear As Boolean?
        Get
            Return Me._Limit2AutoClear
        End Get
        Protected Set(ByVal value As Boolean?)
            If Not Boolean?.Equals(Me.Limit2AutoClear, value) Then
                Me._Limit2AutoClear = value
                Me.SafePostPropertyChanged()
            End If
        End Set
    End Property

    ''' <summary> Writes and reads back the Limit2 Auto Clear sentinel. </summary>
    ''' <param name="value">  if set to <c>True</c> if enabling; False if disabling. </param>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function ApplyLimit2AutoClear(ByVal value As Boolean) As Boolean?
        Me.WriteLimit2AutoClear(value)
        Return Me.QueryLimit2AutoClear()
    End Function

    ''' <summary> Gets or sets the Limit2 Auto Clear query command. </summary>
    ''' <value> The Limit2 Auto Clear query command. </value>
    ''' <remarks> SCPI: ":CALC2:FRES:LIM2:STAT?" </remarks>
    Protected Overridable ReadOnly Property Limit2AutoClearQueryCommand As String

    ''' <summary> Queries the Limit2 Auto Clear sentinel. Also sets the
    ''' <see cref="Limit2AutoClear">AutoClear</see> sentinel. </summary>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function QueryLimit2AutoClear() As Boolean?
        Me.Limit2AutoClear = Me.Query(Me.Limit2AutoClear, Me.Limit2AutoClearQueryCommand)
        Return Me.Limit2AutoClear
    End Function

    ''' <summary> Gets or sets the Limit2 Auto Clear command Format. </summary>
    ''' <value> The Limit2 Auto Clear query command. </value>
    ''' <remarks> SCPI: ":CALC2:FRES:LIM2:STAT {0:'ON';'ON';'OFF'}" </remarks>
    Protected Overridable ReadOnly Property Limit2AutoClearCommandFormat As String

    ''' <summary> Writes the Limit2 Auto Clear sentinel. Does not read back from the instrument. </summary>
    ''' <param name="value"> if set to <c>True</c> is Auto Clear. </param>
    ''' <returns> <c>True</c> if AutoClear; otherwise <c>False</c>. </returns>
    Public Function WriteLimit2AutoClear(ByVal value As Boolean) As Boolean?
        Me.Limit2AutoClear = Me.Write(value, Me.Limit2AutoClearCommandFormat)
        Return Me.Limit2AutoClear
    End Function

#End Region

#End Region

End Class

