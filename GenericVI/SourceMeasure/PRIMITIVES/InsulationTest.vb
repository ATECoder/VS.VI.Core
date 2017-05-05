Imports isr.Core.Pith
''' <summary> An insulation test. </summary>
''' <license>
''' (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
''' Licensed under The MIT License.</para><para>
''' THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.</para>
''' </license>
''' <history date="3/9/2016" by="David" revision=""> Created. </history>
Public Class InsulationTest
    Inherits PropertyPublisherBase
    Implements IPresettablePropertyPublisher

    Public Sub New()
        MyBase.New
        Me._ResetKnowState()
    End Sub

#Region " I PRESETTABLE "

    Private Sub _ResetKnowState()
        Me._Binning = New BinningInfo
        Me._Insulation = New InsulationResistance
        With Me._Binning

            ' not using upper limit
            .UpperLimit = Scpi.Syntax.Infinity
            .UpperLimitFailureBits = 4
            ' using lower limit
            .LowerLimit = 10000000
            .LowerLimitFailureBits = 4
            .PassBits = 1

            .ArmCount = 0
            .ArmDirection = VI.Direction.Source
            ' using SOT Line
            .InputLineNumber = 1
            ' USING EOT Line
            .OutputLineNumber = 2
            .ArmSource = VI.ArmSources.StartTestBoth
            .ArmDirection = VI.Direction.Acceptor
            .TriggerDirection = VI.Direction.Source
            .TriggerSource = VI.TriggerSources.Immediate
        End With

        With Me._Insulation
            .DwellTime = TimeSpan.FromSeconds(2)
            .CurrentLimit = 0.00001
            .PowerLineCycles = 1
            .VoltageLevel = 10
            .ResistanceLowLimit = 10000000
            .ResistanceRange = 1000000000
            .ContactCheckEnabled = True
        End With

    End Sub

    Public Sub ResetKnowState()
        Me._ResetKnowState()
    End Sub

#End Region

    ''' <summary> Publishes this object. </summary>
    Public Overrides Sub Publish() Implements IPresettablePropertyPublisher.Publish
        Me.Binning.Publish()
        Me.Insulation.Publish()
    End Sub

    Public Sub ClearExecutionState() Implements IPresettable.ClearExecutionState
        Throw New NotImplementedException()
    End Sub

    Public Sub InitKnownState() Implements IPresettable.InitKnownState
        Throw New NotImplementedException()
    End Sub

    Public Sub PresetKnownState() Implements IPresettable.PresetKnownState
        Throw New NotImplementedException()
    End Sub

    Public Sub ResetKnownState() Implements IPresettable.ResetKnownState
        Throw New NotImplementedException()
    End Sub

    ''' <summary> Gets or sets the binning. </summary>
    ''' <value> The binning. </value>
    Public Property Binning As BinningInfo

    ''' <summary> Gets or sets the insulation. </summary>
    ''' <value> The insulation. </value>
    Public Property Insulation As InsulationResistance

End Class
