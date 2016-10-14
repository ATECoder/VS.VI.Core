Imports NationalInstruments.Visa

Public Class SelectResources

    Private Overloads Sub OnLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' This example uses an instance of the NationalInstruments.Visa.ResourceManager class to find resources on the system.
        ' Alternatively, static methods provided by the Ivi.Visa.ResourceManager class may be used when an application
        ' requires additional VISA .NET implementations.
        Using rmSession = New ResourceManager()
            Dim resources = rmSession.Find("(ASRL|GPIB|TCPIP|USB)?*")
            For Each s As String In resources
                _AvailableResourcesListBox.Items.Add(s)
            Next s
        End Using
    End Sub

    Private Sub _AddButton_Click(sender As Object, e As EventArgs) Handles _AddButton.Click, _AvailableResourcesListBox.DoubleClick
        Dim selectedString As String = CStr(_AvailableResourcesListBox.SelectedItem)
        Me._ResourceStringsListBox.Items.Add(selectedString)
    End Sub

    Private Sub _RemoveButton_Click(sender As Object, e As EventArgs) Handles _RemoveButton.Click, _ResourceStringsListBox.DoubleClick
        Me._ResourceStringsListBox.Items.Remove(Me._ResourceStringsListBox.SelectedItem)
    End Sub

    Public ReadOnly Property ResourceNames As IEnumerable(Of String)
        Get
            Dim l As New List(Of String)
            For Each item As Object In Me._ResourceStringsListBox.Items
                l.Add(CStr(item))
            Next
            Return l.ToArray
        End Get
    End Property


End Class