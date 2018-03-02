Namespace TspSyntax.SourceMeasureUnit

    ''' <summary> Defines the TSP Source-Measure unit syntax. Modified for TSP2. </summary>
    ''' <license> (c) 2005 Integrated Scientific Resources, Inc.<para>
    ''' Licensed under The MIT License. </para><para>
    ''' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
    ''' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    ''' NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
    ''' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ''' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    ''' </para> </license>
    ''' <history date="01/15/05" by="David" revision="1.0.1841.x"> Created. </history>
    Public Module SourceMeasureUnitSyntax

#Region " NODE "

        Private Const _nodeReferenceFormat As String = "_G.node[{0}]"

        ''' <summary> Returns a TSP reference to the specified node. </summary>
        ''' <param name="node">            Specifies the one-based node number. </param>
        ''' <param name="localNodeNumber"> The local node number. </param>
        ''' <returns> Then node reference. </returns>
        Public Function NodeReference(ByVal node As Integer, ByVal localNodeNumber As Integer) As String

            If node = localNodeNumber Then
                ' if node is number one, use the local node as reference in case
                ' we do not have other nodes.
                Return TspSyntaxConstants.LocalNode
            Else
                Return TspSyntaxConstants.Build(SourceMeasureUnitSyntax._nodeReferenceFormat, node)
            End If

        End Function

#End Region

#Region " SMU "

        ''' <summary> The SMU number 'a'. </summary>
        Public Const SourceMeasureUnitNumberA As String = "a"

        ''' <summary> The SMU number 'b'. </summary>
        Public Const SourceMeasureUnitNumberB As String = "b"

        ''' <summary> The  SMU name format. </summary>
        Private Const _SmuNameFormat As String = "smu{0}"

        ''' <summary> Builds the SMU name, e.g., smua or smub for the specified <paramref name="smuNumber">SMU number</paramref>. </summary>
        ''' <param name="smuNumber">             Specifies the SMU Number (either 'a' or 'b'. </param>
        ''' <returns> rThe node smu reference. </returns>
        Public Function BuildSmuName(ByVal smuNumber As String) As String
            Return TspSyntaxConstants.Build(SourceMeasureUnitSyntax._SmuNameFormat, smuNumber)
        End Function

        ''' <summary> The  global node reference. </summary>
        Private Const _localNodeSmuFormat As String = "_G.localnode.smu{0}"

        Private Const _smuReferenceFormat As String = "_G.node[{0}].smu{1}"

        ''' <summary> Builds a TSP reference to the specified <paramref name="smuNumber">SMU
        ''' number</paramref> on the local node. </summary>
        ''' <param name="smuNumber"> Specifies the SMU (either 'a' or 'b'. </param>
        ''' <returns> rThe node smu reference. </returns>
        Public Function BuildSmuReference(ByVal smuNumber As String) As String

            ' use the local node as reference in case we do not have other nodes.
            Return TspSyntaxConstants.Build(SourceMeasureUnitSyntax._localNodeSmuFormat, smuNumber)

        End Function

        ''' <summary> Builds a TSP reference to the specified <paramref name="smuNumber">SMU
        ''' number</paramref> on the specified node. </summary>
        ''' <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        ''' illegal values. </exception>
        ''' <param name="nodeNumber">      Specifies the node number (must be greater than 0). </param>
        ''' <param name="localNodeNumber"> The local node number (must be greater than 0). </param>
        ''' <param name="smuNumber">       Specifies the SMU (either 'a' or 'b'. </param>
        ''' <returns> rThe node smu reference. </returns>
        Public Function BuildSmuReference(ByVal nodeNumber As Integer, ByVal localNodeNumber As Integer, ByVal smuNumber As String) As String

            If nodeNumber <= 0 Then
                Throw New ArgumentException("Node number must be greater than Or equal To 1.")
            ElseIf localNodeNumber <= 0 Then
                Throw New ArgumentException("Local node number must be greater than Or equal To 1.")
            End If
            If nodeNumber = localNodeNumber Then
                ' if node is number one, use the local node as reference in case
                ' we do not have other nodes.
                Return SourceMeasureUnitSyntax.BuildSmuReference(smuNumber)
            Else
                Return TspSyntaxConstants.Build(SourceMeasureUnitSyntax._smuReferenceFormat, nodeNumber, smuNumber)
            End If

        End Function
#End Region

    End Module
End Namespace

