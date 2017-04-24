<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TestPanel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Private _ToolTip As System.Windows.Forms.ToolTip
    Private WithEvents _AboutButton As System.Windows.Forms.Button
    Private WithEvents _ExitButton As System.Windows.Forms.Button
    Private WithEvents _ErrorProvider As ErrorProvider
    Private WithEvents _OutputTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ExecutionTimeTextBox As System.Windows.Forms.TextBox
    Private WithEvents _InputTextBox As System.Windows.Forms.TextBox
    Private WithEvents _ExecutionTimeTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _ReplyTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _QueryCommandTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _ConsoleTabPage As System.Windows.Forms.TabPage
    Private WithEvents _LoadAndRunButton As System.Windows.Forms.Button
    Private WithEvents _RunScriptButton As System.Windows.Forms.Button
    Private WithEvents _LoadScriptButton As System.Windows.Forms.Button
    Private WithEvents _RemoveScriptButton As System.Windows.Forms.Button
    Private WithEvents _UserScriptsList As System.Windows.Forms.ListBox
    Private WithEvents _ScriptNameTextBox As System.Windows.Forms.TextBox
    Private WithEvents _TspScriptSelector As isr.Core.Controls.FileSelector
    Private WithEvents _UserScriptsListLabel As System.Windows.Forms.Label
    Private WithEvents _TspScriptSelectorLabel As System.Windows.Forms.Label
    Private WithEvents _ScriptNameTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _ScriptsTabPage As System.Windows.Forms.TabPage
    Private WithEvents _FunctionArgsTextBox As System.Windows.Forms.TextBox
    Private WithEvents _CallFunctionButton As System.Windows.Forms.Button
    Private WithEvents _FunctionCodeTextBox As System.Windows.Forms.TextBox
    Private WithEvents _FunctionNameTextBox As System.Windows.Forms.TextBox
    Private WithEvents _LoadFunctionButton As System.Windows.Forms.Button
    Private WithEvents _FunctionArgsTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _CallInstructionsLabel As System.Windows.Forms.Label
    Private WithEvents _FunctionCodeTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _FunctionNameTextBoxLabel As System.Windows.Forms.Label
    Private WithEvents _FunctionsTabPage As System.Windows.Forms.TabPage
    Private WithEvents _ResetLocalNodeButton As System.Windows.Forms.Button
    Private WithEvents _ShowPromptsCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents _ShowErrorsCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents _AbortButton As System.Windows.Forms.Button
    Private WithEvents _GroupTriggerButton As System.Windows.Forms.Button
    Private WithEvents _DeviceClearButton As System.Windows.Forms.Button
    Private WithEvents _InstrumentTabPage As System.Windows.Forms.TabPage
    Private WithEvents _TraceMessagesBox As isr.Core.Pith.TraceMessagesBox
    Private WithEvents _MessagesTabPage As System.Windows.Forms.TabPage
    Private WithEvents _TabControl As System.Windows.Forms.TabControl
    Private WithEvents _StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _StatusStrip As System.Windows.Forms.StatusStrip
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TestPanel))
        Me._toolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._outputTextBox = New System.Windows.Forms.TextBox()
        Me._executionTimeTextBox = New System.Windows.Forms.TextBox()
        Me._loadAndRunButton = New System.Windows.Forms.Button()
        Me._runScriptButton = New System.Windows.Forms.Button()
        Me._loadScriptButton = New System.Windows.Forms.Button()
        Me._removeScriptButton = New System.Windows.Forms.Button()
        Me._userScriptsList = New System.Windows.Forms.ListBox()
        Me._scriptNameTextBox = New System.Windows.Forms.TextBox()
        Me._callFunctionButton = New System.Windows.Forms.Button()
        Me._loadFunctionButton = New System.Windows.Forms.Button()
        Me._resetLocalNodeButton = New System.Windows.Forms.Button()
        Me._showPromptsCheckBox = New System.Windows.Forms.CheckBox()
        Me._showErrorsCheckBox = New System.Windows.Forms.CheckBox()
        Me._abortButton = New System.Windows.Forms.Button()
        Me._groupTriggerButton = New System.Windows.Forms.Button()
        Me._deviceClearButton = New System.Windows.Forms.Button()
        Me._clearInterfaceButton = New System.Windows.Forms.Button()
        Me._tspScriptSelector = New isr.Core.Controls.FileSelector()
        Me._controlsTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me._controlsTableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me._aboutButton = New System.Windows.Forms.Button()
        Me._ExitButton = New System.Windows.Forms.Button()
        Me._ResourceSelectorConnector = New isr.VI.Instrument.ResourceSelectorConnector()
        Me._errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._tabControl = New System.Windows.Forms.TabControl()
        Me._consoleTabPage = New System.Windows.Forms.TabPage()
        Me._consoleTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me._inputPanel = New System.Windows.Forms.Panel()
        Me._inputTextBox = New System.Windows.Forms.TextBox()
        Me._timingTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me._queryCommandTextBoxLabel = New System.Windows.Forms.Label()
        Me._executionTimeTextBoxLabel = New System.Windows.Forms.Label()
        Me._outputPanel = New System.Windows.Forms.Panel()
        Me._replyTextBoxLabel = New System.Windows.Forms.Label()
        Me._scriptsTabPage = New System.Windows.Forms.TabPage()
        Me._scriptsTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me._scriptsPanel4 = New System.Windows.Forms.Panel()
        Me._userScriptsListLabel = New System.Windows.Forms.Label()
        Me._refreshUserScriptsListButton = New System.Windows.Forms.Button()
        Me._scriptsPanel3 = New System.Windows.Forms.Panel()
        Me._tspScriptSelectorLabel = New System.Windows.Forms.Label()
        Me._scriptsPanel2 = New System.Windows.Forms.Panel()
        Me._retainCodeOutlineToggle = New System.Windows.Forms.CheckBox()
        Me._scriptNameTextBoxLabel = New System.Windows.Forms.Label()
        Me._scriptsPanel1 = New System.Windows.Forms.Panel()
        Me._functionsTabPage = New System.Windows.Forms.TabPage()
        Me._functionsTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me._functionCodeTextBox = New System.Windows.Forms.TextBox()
        Me._functionsPanel1 = New System.Windows.Forms.Panel()
        Me._functionArgsTextBox = New System.Windows.Forms.TextBox()
        Me._functionNameTextBox = New System.Windows.Forms.TextBox()
        Me._functionNameTextBoxLabel = New System.Windows.Forms.Label()
        Me._functionArgsTextBoxLabel = New System.Windows.Forms.Label()
        Me._functionsTableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel()
        Me._functionCodeTextBoxLabel = New System.Windows.Forms.Label()
        Me._callInstructionsLabel = New System.Windows.Forms.Label()
        Me._instrumentTabPage = New System.Windows.Forms.TabPage()
        Me._instrumentTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me._instrumentPanel3 = New System.Windows.Forms.Panel()
        Me._instrumentPanel2 = New System.Windows.Forms.Panel()
        Me._instrumentPanel1 = New System.Windows.Forms.Panel()
        Me._messagesTabPage = New System.Windows.Forms.TabPage()
        Me._TraceMessagesBox = New isr.Core.Pith.TraceMessagesBox()
        Me._statusStrip = New System.Windows.Forms.StatusStrip()
        Me._statusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._srqStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._tspStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me._controlsTableLayoutPanel.SuspendLayout()
        Me._controlsTableLayoutPanel1.SuspendLayout()
        CType(Me._errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabControl.SuspendLayout()
        Me._consoleTabPage.SuspendLayout()
        Me._consoleTableLayoutPanel.SuspendLayout()
        Me._inputPanel.SuspendLayout()
        Me._timingTableLayoutPanel.SuspendLayout()
        Me._outputPanel.SuspendLayout()
        Me._scriptsTabPage.SuspendLayout()
        Me._scriptsTableLayoutPanel.SuspendLayout()
        Me._scriptsPanel4.SuspendLayout()
        Me._scriptsPanel3.SuspendLayout()
        Me._scriptsPanel2.SuspendLayout()
        Me._scriptsPanel1.SuspendLayout()
        Me._functionsTabPage.SuspendLayout()
        Me._functionsTableLayoutPanel.SuspendLayout()
        Me._functionsPanel1.SuspendLayout()
        Me._functionsTableLayoutPanel5.SuspendLayout()
        Me._instrumentTabPage.SuspendLayout()
        Me._instrumentTableLayoutPanel.SuspendLayout()
        Me._instrumentPanel3.SuspendLayout()
        Me._instrumentPanel2.SuspendLayout()
        Me._instrumentPanel1.SuspendLayout()
        Me._messagesTabPage.SuspendLayout()
        Me._statusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        '_outputTextBox
        '
        Me._outputTextBox.AcceptsReturn = True
        Me._outputTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me._outputTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._outputTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me._outputTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._outputTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me._outputTextBox.Location = New System.Drawing.Point(0, 32)
        Me._outputTextBox.MaxLength = 0
        Me._outputTextBox.Multiline = True
        Me._outputTextBox.Name = "_outputTextBox"
        Me._outputTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._outputTextBox.Size = New System.Drawing.Size(386, 436)
        Me._outputTextBox.TabIndex = 1
        Me._toolTip.SetToolTip(Me._outputTextBox, "Output from the instrument")
        Me._outputTextBox.WordWrap = False
        '
        '_executionTimeTextBox
        '
        Me._executionTimeTextBox.AcceptsReturn = True
        Me._executionTimeTextBox.BackColor = System.Drawing.SystemColors.Window
        Me._executionTimeTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._executionTimeTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me._executionTimeTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me._executionTimeTextBox.Location = New System.Drawing.Point(309, 3)
        Me._executionTimeTextBox.MaxLength = 0
        Me._executionTimeTextBox.Name = "_executionTimeTextBox"
        Me._executionTimeTextBox.ReadOnly = True
        Me._executionTimeTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._executionTimeTextBox.Size = New System.Drawing.Size(74, 25)
        Me._executionTimeTextBox.TabIndex = 37
        Me._executionTimeTextBox.Text = "0.000"
        Me._toolTip.SetToolTip(Me._executionTimeTextBox, "Execution time of last command in milli seconds")
        '
        '_loadAndRunButton
        '
        Me._loadAndRunButton.BackColor = System.Drawing.SystemColors.Control
        Me._loadAndRunButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._loadAndRunButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._loadAndRunButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._loadAndRunButton.Location = New System.Drawing.Point(15, 86)
        Me._loadAndRunButton.Name = "_loadAndRunButton"
        Me._loadAndRunButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._loadAndRunButton.Size = New System.Drawing.Size(145, 33)
        Me._loadAndRunButton.TabIndex = 2
        Me._loadAndRunButton.Text = "L&OAD  AND  RUN"
        Me._toolTip.SetToolTip(Me._loadAndRunButton, "Loads and runs the script.")
        Me._loadAndRunButton.UseVisualStyleBackColor = True
        '
        '_runScriptButton
        '
        Me._runScriptButton.BackColor = System.Drawing.SystemColors.Control
        Me._runScriptButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._runScriptButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._runScriptButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._runScriptButton.Location = New System.Drawing.Point(15, 50)
        Me._runScriptButton.Name = "_runScriptButton"
        Me._runScriptButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._runScriptButton.Size = New System.Drawing.Size(145, 33)
        Me._runScriptButton.TabIndex = 1
        Me._runScriptButton.Text = "&RUN"
        Me._toolTip.SetToolTip(Me._runScriptButton, "Runs the script")
        Me._runScriptButton.UseVisualStyleBackColor = True
        '
        '_loadScriptButton
        '
        Me._loadScriptButton.BackColor = System.Drawing.SystemColors.Control
        Me._loadScriptButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._loadScriptButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._loadScriptButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._loadScriptButton.Location = New System.Drawing.Point(15, 14)
        Me._loadScriptButton.Name = "_loadScriptButton"
        Me._loadScriptButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._loadScriptButton.Size = New System.Drawing.Size(145, 33)
        Me._loadScriptButton.TabIndex = 0
        Me._loadScriptButton.Text = "&LOAD"
        Me._toolTip.SetToolTip(Me._loadScriptButton, "Loads the script")
        Me._loadScriptButton.UseVisualStyleBackColor = True
        '
        '_removeScriptButton
        '
        Me._removeScriptButton.BackColor = System.Drawing.SystemColors.Control
        Me._removeScriptButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._removeScriptButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._removeScriptButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._removeScriptButton.Location = New System.Drawing.Point(4, 176)
        Me._removeScriptButton.Name = "_removeScriptButton"
        Me._removeScriptButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._removeScriptButton.Size = New System.Drawing.Size(84, 33)
        Me._removeScriptButton.TabIndex = 0
        Me._removeScriptButton.Text = "&REMOVE"
        Me._toolTip.SetToolTip(Me._removeScriptButton, "Click to remove a selected script from the instrument.  This does not remove the " &
        "script functions.")
        Me._removeScriptButton.UseVisualStyleBackColor = True
        '
        '_userScriptsList
        '
        Me._userScriptsList.BackColor = System.Drawing.SystemColors.Window
        Me._userScriptsList.Cursor = System.Windows.Forms.Cursors.Default
        Me._userScriptsList.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._userScriptsList.ForeColor = System.Drawing.SystemColors.WindowText
        Me._userScriptsList.ItemHeight = 17
        Me._userScriptsList.Location = New System.Drawing.Point(4, 28)
        Me._userScriptsList.Name = "_userScriptsList"
        Me._userScriptsList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._userScriptsList.Size = New System.Drawing.Size(173, 140)
        Me._userScriptsList.TabIndex = 2
        Me._toolTip.SetToolTip(Me._userScriptsList, "Lists the user scripts currently saved in the instrument")
        '
        '_scriptNameTextBox
        '
        Me._scriptNameTextBox.AcceptsReturn = True
        Me._scriptNameTextBox.BackColor = System.Drawing.SystemColors.Window
        Me._scriptNameTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._scriptNameTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._scriptNameTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me._scriptNameTextBox.Location = New System.Drawing.Point(4, 27)
        Me._scriptNameTextBox.MaxLength = 0
        Me._scriptNameTextBox.Name = "_scriptNameTextBox"
        Me._scriptNameTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._scriptNameTextBox.Size = New System.Drawing.Size(269, 25)
        Me._scriptNameTextBox.TabIndex = 1
        Me._scriptNameTextBox.Text = "product.tsp"
        Me._toolTip.SetToolTip(Me._scriptNameTextBox, "Enter the name under which the script is saved in the instrument")
        '
        '_callFunctionButton
        '
        Me._callFunctionButton.BackColor = System.Drawing.SystemColors.Control
        Me._callFunctionButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._callFunctionButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._callFunctionButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._callFunctionButton.Location = New System.Drawing.Point(415, 17)
        Me._callFunctionButton.Name = "_callFunctionButton"
        Me._callFunctionButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._callFunctionButton.Size = New System.Drawing.Size(93, 33)
        Me._callFunctionButton.TabIndex = 4
        Me._callFunctionButton.Text = "&CALL"
        Me._toolTip.SetToolTip(Me._callFunctionButton, "Runs (executes) the function using pcall.")
        Me._callFunctionButton.UseVisualStyleBackColor = True
        '
        '_loadFunctionButton
        '
        Me._loadFunctionButton.BackColor = System.Drawing.SystemColors.Control
        Me._loadFunctionButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._loadFunctionButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._loadFunctionButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._loadFunctionButton.Location = New System.Drawing.Point(634, 3)
        Me._loadFunctionButton.Name = "_loadFunctionButton"
        Me._loadFunctionButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._loadFunctionButton.Size = New System.Drawing.Size(129, 27)
        Me._loadFunctionButton.TabIndex = 1
        Me._loadFunctionButton.Text = "&LOAD FUNCTION"
        Me._toolTip.SetToolTip(Me._loadFunctionButton, "Loads the function")
        Me._loadFunctionButton.UseVisualStyleBackColor = True
        '
        '_resetLocalNodeButton
        '
        Me._resetLocalNodeButton.BackColor = System.Drawing.SystemColors.Control
        Me._resetLocalNodeButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._resetLocalNodeButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._resetLocalNodeButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._resetLocalNodeButton.Location = New System.Drawing.Point(8, 64)
        Me._resetLocalNodeButton.Name = "_resetLocalNodeButton"
        Me._resetLocalNodeButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._resetLocalNodeButton.Size = New System.Drawing.Size(173, 33)
        Me._resetLocalNodeButton.TabIndex = 1
        Me._resetLocalNodeButton.Text = "&RESET LOCAL NODE"
        Me._toolTip.SetToolTip(Me._resetLocalNodeButton, "Resets the local node clearing the error queue")
        Me._resetLocalNodeButton.UseVisualStyleBackColor = True
        '
        '_showPromptsCheckBox
        '
        Me._showPromptsCheckBox.BackColor = System.Drawing.Color.Transparent
        Me._showPromptsCheckBox.Cursor = System.Windows.Forms.Cursors.Default
        Me._showPromptsCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._showPromptsCheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me._showPromptsCheckBox.Location = New System.Drawing.Point(7, 22)
        Me._showPromptsCheckBox.Name = "_showPromptsCheckBox"
        Me._showPromptsCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._showPromptsCheckBox.Size = New System.Drawing.Size(185, 21)
        Me._showPromptsCheckBox.TabIndex = 0
        Me._showPromptsCheckBox.Text = "ENABLE &PROMPTS"
        Me._showPromptsCheckBox.ThreeState = True
        Me._toolTip.SetToolTip(Me._showPromptsCheckBox, "Check to have the instrument send a prompt after each command.")
        Me._showPromptsCheckBox.UseVisualStyleBackColor = False
        '
        '_showErrorsCheckBox
        '
        Me._showErrorsCheckBox.BackColor = System.Drawing.Color.Transparent
        Me._showErrorsCheckBox.Cursor = System.Windows.Forms.Cursors.Default
        Me._showErrorsCheckBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._showErrorsCheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me._showErrorsCheckBox.Location = New System.Drawing.Point(7, 70)
        Me._showErrorsCheckBox.Name = "_showErrorsCheckBox"
        Me._showErrorsCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._showErrorsCheckBox.Size = New System.Drawing.Size(185, 21)
        Me._showErrorsCheckBox.TabIndex = 1
        Me._showErrorsCheckBox.Text = "ENABLE &ERROR OUTPUT"
        Me._showErrorsCheckBox.ThreeState = True
        Me._toolTip.SetToolTip(Me._showErrorsCheckBox, "Check to have the instrument send error messages")
        Me._showErrorsCheckBox.UseVisualStyleBackColor = True
        '
        '_abortButton
        '
        Me._abortButton.BackColor = System.Drawing.SystemColors.Control
        Me._abortButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._abortButton.Enabled = False
        Me._abortButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._abortButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._abortButton.Location = New System.Drawing.Point(8, 16)
        Me._abortButton.Name = "_abortButton"
        Me._abortButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._abortButton.Size = New System.Drawing.Size(173, 33)
        Me._abortButton.TabIndex = 0
        Me._abortButton.Text = "&ABORT ACTIVE SCRIPT"
        Me._toolTip.SetToolTip(Me._abortButton, "Aborts execution of the current script.")
        Me._abortButton.UseVisualStyleBackColor = True
        '
        '_groupTriggerButton
        '
        Me._groupTriggerButton.BackColor = System.Drawing.SystemColors.Control
        Me._groupTriggerButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._groupTriggerButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._groupTriggerButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._groupTriggerButton.Location = New System.Drawing.Point(9, 64)
        Me._groupTriggerButton.Name = "_groupTriggerButton"
        Me._groupTriggerButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._groupTriggerButton.Size = New System.Drawing.Size(145, 33)
        Me._groupTriggerButton.TabIndex = 1
        Me._groupTriggerButton.Text = "ASSERT &TRIGGER"
        Me._toolTip.SetToolTip(Me._groupTriggerButton, "Aborts the current command.")
        Me._groupTriggerButton.UseVisualStyleBackColor = True
        '
        '_deviceClearButton
        '
        Me._deviceClearButton.BackColor = System.Drawing.SystemColors.Control
        Me._deviceClearButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._deviceClearButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._deviceClearButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._deviceClearButton.Location = New System.Drawing.Point(9, 16)
        Me._deviceClearButton.Name = "_deviceClearButton"
        Me._deviceClearButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._deviceClearButton.Size = New System.Drawing.Size(145, 33)
        Me._deviceClearButton.TabIndex = 0
        Me._deviceClearButton.Text = "CLEAR &DEVICE"
        Me._toolTip.SetToolTip(Me._deviceClearButton, "Aborts the current command.")
        Me._deviceClearButton.UseVisualStyleBackColor = True
        '
        '_clearInterfaceButton
        '
        Me._clearInterfaceButton.BackColor = System.Drawing.SystemColors.Control
        Me._clearInterfaceButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._clearInterfaceButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._clearInterfaceButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._clearInterfaceButton.Location = New System.Drawing.Point(78, 3)
        Me._clearInterfaceButton.Name = "_clearInterfaceButton"
        Me._clearInterfaceButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._clearInterfaceButton.Size = New System.Drawing.Size(123, 31)
        Me._clearInterfaceButton.TabIndex = 2
        Me._clearInterfaceButton.Text = "CLEAR &INTERFACE"
        Me._toolTip.SetToolTip(Me._clearInterfaceButton, "Clears the GPIB interface")
        Me._clearInterfaceButton.UseVisualStyleBackColor = True
        '
        '_tspScriptSelector
        '
        Me._tspScriptSelector.BackColor = System.Drawing.SystemColors.Window
        Me._tspScriptSelector.DefaultExtension = ".TSP"
        Me._tspScriptSelector.DialogFilter = "TSP Files (*.tsp; *.lua)|*.tsp;*.lua|Script Files (*.dbg)|*.dbg|All Files (*.*)|*" &
    ".*"
        Me._tspScriptSelector.DialogTitle = "SELECT TSP SCRIPT FILE"
        Me._tspScriptSelector.Dock = System.Windows.Forms.DockStyle.Top
        Me._tspScriptSelector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._tspScriptSelector.Location = New System.Drawing.Point(3, 22)
        Me._tspScriptSelector.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me._tspScriptSelector.Name = "_tspScriptSelector"
        Me._tspScriptSelector.Padding = New System.Windows.Forms.Padding(3)
        Me._tspScriptSelector.Size = New System.Drawing.Size(598, 27)
        Me._tspScriptSelector.TabIndex = 0
        Me._toolTip.SetToolTip(Me._tspScriptSelector, "The file holding the TSP script code.")
        '
        '_controlsTableLayoutPanel
        '
        Me._controlsTableLayoutPanel.BackColor = System.Drawing.Color.Transparent
        Me._controlsTableLayoutPanel.ColumnCount = 1
        Me._controlsTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._controlsTableLayoutPanel.Controls.Add(Me._controlsTableLayoutPanel1, 0, 1)
        Me._controlsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me._controlsTableLayoutPanel.Location = New System.Drawing.Point(0, 500)
        Me._controlsTableLayoutPanel.Name = "_controlsTableLayoutPanel"
        Me._controlsTableLayoutPanel.RowCount = 3
        Me._controlsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._controlsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._controlsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._controlsTableLayoutPanel.Size = New System.Drawing.Size(792, 41)
        Me._controlsTableLayoutPanel.TabIndex = 55
        '
        '_controlsTableLayoutPanel1
        '
        Me._controlsTableLayoutPanel1.ColumnCount = 6
        Me._controlsTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
        Me._controlsTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._controlsTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._controlsTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._controlsTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._controlsTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
        Me._controlsTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._controlsTableLayoutPanel1.Controls.Add(Me._aboutButton, 1, 0)
        Me._controlsTableLayoutPanel1.Controls.Add(Me._clearInterfaceButton, 2, 0)
        Me._controlsTableLayoutPanel1.Controls.Add(Me._ExitButton, 4, 0)
        Me._controlsTableLayoutPanel1.Controls.Add(Me._ResourceSelectorConnector, 3, 0)
        Me._controlsTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me._controlsTableLayoutPanel1.Location = New System.Drawing.Point(3, 2)
        Me._controlsTableLayoutPanel1.Name = "_controlsTableLayoutPanel1"
        Me._controlsTableLayoutPanel1.RowCount = 1
        Me._controlsTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._controlsTableLayoutPanel1.Size = New System.Drawing.Size(786, 37)
        Me._controlsTableLayoutPanel1.TabIndex = 54
        '
        '_aboutButton
        '
        Me._aboutButton.BackColor = System.Drawing.SystemColors.Control
        Me._aboutButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._aboutButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._aboutButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._aboutButton.Location = New System.Drawing.Point(8, 3)
        Me._aboutButton.Name = "_aboutButton"
        Me._aboutButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._aboutButton.Size = New System.Drawing.Size(64, 31)
        Me._aboutButton.TabIndex = 0
        Me._aboutButton.Text = "&ABOUT"
        Me._aboutButton.UseVisualStyleBackColor = True
        '
        '_ExitButton
        '
        Me._ExitButton.BackColor = System.Drawing.SystemColors.Control
        Me._ExitButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._ExitButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._ExitButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._ExitButton.Location = New System.Drawing.Point(702, 3)
        Me._ExitButton.Name = "_ExitButton"
        Me._ExitButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._ExitButton.Size = New System.Drawing.Size(76, 31)
        Me._ExitButton.TabIndex = 5
        Me._ExitButton.Text = "E&XIT"
        Me._ExitButton.UseVisualStyleBackColor = True
        '
        '_ResourceSelectorConnector
        '
        Me._ResourceSelectorConnector.BackColor = System.Drawing.Color.Transparent
        Me._ResourceSelectorConnector.Clearable = False
        Me._ResourceSelectorConnector.Connectable = False
        Me._ResourceSelectorConnector.Dock = System.Windows.Forms.DockStyle.Fill
        Me._ResourceSelectorConnector.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ResourceSelectorConnector.Location = New System.Drawing.Point(205, 1)
        Me._ResourceSelectorConnector.Margin = New System.Windows.Forms.Padding(1)
        Me._ResourceSelectorConnector.Name = "_ResourceSelectorConnector"
        Me._ResourceSelectorConnector.Searchable = False
        Me._ResourceSelectorConnector.Size = New System.Drawing.Size(493, 35)
        Me._ResourceSelectorConnector.TabIndex = 6
        '
        '_errorProvider
        '
        Me._errorProvider.ContainerControl = Me
        '
        '_tabControl
        '
        Me._tabControl.Controls.Add(Me._consoleTabPage)
        Me._tabControl.Controls.Add(Me._scriptsTabPage)
        Me._tabControl.Controls.Add(Me._functionsTabPage)
        Me._tabControl.Controls.Add(Me._instrumentTabPage)
        Me._tabControl.Controls.Add(Me._messagesTabPage)
        Me._tabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me._tabControl.ItemSize = New System.Drawing.Size(42, 22)
        Me._tabControl.Location = New System.Drawing.Point(0, 0)
        Me._tabControl.Name = "_tabControl"
        Me._tabControl.Padding = New System.Drawing.Point(15, 3)
        Me._tabControl.SelectedIndex = 0
        Me._tabControl.Size = New System.Drawing.Size(792, 500)
        Me._tabControl.TabIndex = 1
        '
        '_consoleTabPage
        '
        Me._consoleTabPage.Controls.Add(Me._consoleTableLayoutPanel)
        Me._consoleTabPage.Location = New System.Drawing.Point(4, 22)
        Me._consoleTabPage.Name = "_consoleTabPage"
        Me._consoleTabPage.Size = New System.Drawing.Size(784, 474)
        Me._consoleTabPage.TabIndex = 0
        Me._consoleTabPage.Text = "CONSOLE"
        Me._consoleTabPage.UseVisualStyleBackColor = True
        '
        '_consoleTableLayoutPanel
        '
        Me._consoleTableLayoutPanel.ColumnCount = 2
        Me._consoleTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._consoleTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._consoleTableLayoutPanel.Controls.Add(Me._inputPanel, 0, 0)
        Me._consoleTableLayoutPanel.Controls.Add(Me._outputPanel, 1, 0)
        Me._consoleTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._consoleTableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me._consoleTableLayoutPanel.Name = "_consoleTableLayoutPanel"
        Me._consoleTableLayoutPanel.RowCount = 1
        Me._consoleTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._consoleTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 474.0!))
        Me._consoleTableLayoutPanel.Size = New System.Drawing.Size(784, 474)
        Me._consoleTableLayoutPanel.TabIndex = 8
        '
        '_inputPanel
        '
        Me._inputPanel.Controls.Add(Me._inputTextBox)
        Me._inputPanel.Controls.Add(Me._timingTableLayoutPanel)
        Me._inputPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._inputPanel.Location = New System.Drawing.Point(3, 3)
        Me._inputPanel.Name = "_inputPanel"
        Me._inputPanel.Size = New System.Drawing.Size(386, 468)
        Me._inputPanel.TabIndex = 41
        '
        '_inputTextBox
        '
        Me._inputTextBox.AcceptsReturn = True
        Me._inputTextBox.BackColor = System.Drawing.SystemColors.Window
        Me._inputTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._inputTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me._inputTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._inputTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me._inputTextBox.Location = New System.Drawing.Point(0, 33)
        Me._inputTextBox.MaxLength = 0
        Me._inputTextBox.Multiline = True
        Me._inputTextBox.Name = "_inputTextBox"
        Me._inputTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._inputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._inputTextBox.Size = New System.Drawing.Size(386, 435)
        Me._inputTextBox.TabIndex = 0
        Me._inputTextBox.Text = "print(_VERSION)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me._inputTextBox.WordWrap = False
        '
        '_timingTableLayoutPanel
        '
        Me._timingTableLayoutPanel.ColumnCount = 3
        Me._timingTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._timingTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._timingTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._timingTableLayoutPanel.Controls.Add(Me._executionTimeTextBox, 2, 0)
        Me._timingTableLayoutPanel.Controls.Add(Me._queryCommandTextBoxLabel, 0, 0)
        Me._timingTableLayoutPanel.Controls.Add(Me._executionTimeTextBoxLabel, 1, 0)
        Me._timingTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me._timingTableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me._timingTableLayoutPanel.Name = "_timingTableLayoutPanel"
        Me._timingTableLayoutPanel.RowCount = 1
        Me._timingTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._timingTableLayoutPanel.Size = New System.Drawing.Size(386, 33)
        Me._timingTableLayoutPanel.TabIndex = 42
        '
        '_queryCommandTextBoxLabel
        '
        Me._queryCommandTextBoxLabel.AutoSize = True
        Me._queryCommandTextBoxLabel.BackColor = System.Drawing.Color.Transparent
        Me._queryCommandTextBoxLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me._queryCommandTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._queryCommandTextBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me._queryCommandTextBoxLabel.Location = New System.Drawing.Point(3, 0)
        Me._queryCommandTextBoxLabel.Name = "_queryCommandTextBoxLabel"
        Me._queryCommandTextBoxLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._queryCommandTextBoxLabel.Size = New System.Drawing.Size(147, 33)
        Me._queryCommandTextBoxLabel.TabIndex = 0
        Me._queryCommandTextBoxLabel.Text = "Instrument Input: "
        Me._queryCommandTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        '_executionTimeTextBoxLabel
        '
        Me._executionTimeTextBoxLabel.AutoSize = True
        Me._executionTimeTextBoxLabel.BackColor = System.Drawing.Color.Transparent
        Me._executionTimeTextBoxLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me._executionTimeTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._executionTimeTextBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me._executionTimeTextBoxLabel.Location = New System.Drawing.Point(156, 0)
        Me._executionTimeTextBoxLabel.Name = "_executionTimeTextBoxLabel"
        Me._executionTimeTextBoxLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._executionTimeTextBoxLabel.Size = New System.Drawing.Size(147, 33)
        Me._executionTimeTextBoxLabel.TabIndex = 38
        Me._executionTimeTextBoxLabel.Text = "Execution Time [ms]:"
        Me._executionTimeTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_outputPanel
        '
        Me._outputPanel.Controls.Add(Me._outputTextBox)
        Me._outputPanel.Controls.Add(Me._replyTextBoxLabel)
        Me._outputPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._outputPanel.Location = New System.Drawing.Point(395, 3)
        Me._outputPanel.Name = "_outputPanel"
        Me._outputPanel.Size = New System.Drawing.Size(386, 468)
        Me._outputPanel.TabIndex = 40
        '
        '_replyTextBoxLabel
        '
        Me._replyTextBoxLabel.BackColor = System.Drawing.Color.Transparent
        Me._replyTextBoxLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me._replyTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me._replyTextBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me._replyTextBoxLabel.Location = New System.Drawing.Point(0, 0)
        Me._replyTextBoxLabel.Name = "_replyTextBoxLabel"
        Me._replyTextBoxLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._replyTextBoxLabel.Size = New System.Drawing.Size(386, 32)
        Me._replyTextBoxLabel.TabIndex = 0
        Me._replyTextBoxLabel.Text = "Instrument Output: "
        Me._replyTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        '_scriptsTabPage
        '
        Me._scriptsTabPage.Controls.Add(Me._scriptsTableLayoutPanel)
        Me._scriptsTabPage.Controls.Add(Me._scriptsPanel1)
        Me._scriptsTabPage.Location = New System.Drawing.Point(4, 22)
        Me._scriptsTabPage.Name = "_scriptsTabPage"
        Me._scriptsTabPage.Size = New System.Drawing.Size(784, 474)
        Me._scriptsTabPage.TabIndex = 1
        Me._scriptsTabPage.Text = "SCRIPTS"
        Me._scriptsTabPage.UseVisualStyleBackColor = True
        '
        '_scriptsTableLayoutPanel
        '
        Me._scriptsTableLayoutPanel.ColumnCount = 1
        Me._scriptsTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._scriptsTableLayoutPanel.Controls.Add(Me._scriptsPanel4, 0, 5)
        Me._scriptsTableLayoutPanel.Controls.Add(Me._scriptsPanel3, 0, 3)
        Me._scriptsTableLayoutPanel.Controls.Add(Me._scriptsPanel2, 0, 1)
        Me._scriptsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._scriptsTableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me._scriptsTableLayoutPanel.Name = "_scriptsTableLayoutPanel"
        Me._scriptsTableLayoutPanel.RowCount = 7
        Me._scriptsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._scriptsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._scriptsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._scriptsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._scriptsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._scriptsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._scriptsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._scriptsTableLayoutPanel.Size = New System.Drawing.Size(610, 474)
        Me._scriptsTableLayoutPanel.TabIndex = 41
        '
        '_scriptsPanel4
        '
        Me._scriptsPanel4.Controls.Add(Me._userScriptsListLabel)
        Me._scriptsPanel4.Controls.Add(Me._userScriptsList)
        Me._scriptsPanel4.Controls.Add(Me._refreshUserScriptsListButton)
        Me._scriptsPanel4.Controls.Add(Me._removeScriptButton)
        Me._scriptsPanel4.Location = New System.Drawing.Point(3, 191)
        Me._scriptsPanel4.Name = "_scriptsPanel4"
        Me._scriptsPanel4.Size = New System.Drawing.Size(191, 220)
        Me._scriptsPanel4.TabIndex = 40
        '
        '_userScriptsListLabel
        '
        Me._userScriptsListLabel.BackColor = System.Drawing.Color.Transparent
        Me._userScriptsListLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me._userScriptsListLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me._userScriptsListLabel.Location = New System.Drawing.Point(4, 8)
        Me._userScriptsListLabel.Name = "_userScriptsListLabel"
        Me._userScriptsListLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._userScriptsListLabel.Size = New System.Drawing.Size(173, 16)
        Me._userScriptsListLabel.TabIndex = 18
        Me._userScriptsListLabel.Text = "User Scripts: "
        '
        '_refreshUserScriptsListButton
        '
        Me._refreshUserScriptsListButton.BackColor = System.Drawing.SystemColors.Control
        Me._refreshUserScriptsListButton.Cursor = System.Windows.Forms.Cursors.Default
        Me._refreshUserScriptsListButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._refreshUserScriptsListButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me._refreshUserScriptsListButton.Location = New System.Drawing.Point(93, 176)
        Me._refreshUserScriptsListButton.Name = "_refreshUserScriptsListButton"
        Me._refreshUserScriptsListButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._refreshUserScriptsListButton.Size = New System.Drawing.Size(84, 33)
        Me._refreshUserScriptsListButton.TabIndex = 0
        Me._refreshUserScriptsListButton.Text = "REFRES&H"
        Me._refreshUserScriptsListButton.UseVisualStyleBackColor = True
        '
        '_scriptsPanel3
        '
        Me._scriptsPanel3.Controls.Add(Me._tspScriptSelector)
        Me._scriptsPanel3.Controls.Add(Me._tspScriptSelectorLabel)
        Me._scriptsPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me._scriptsPanel3.Location = New System.Drawing.Point(3, 108)
        Me._scriptsPanel3.Name = "_scriptsPanel3"
        Me._scriptsPanel3.Padding = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me._scriptsPanel3.Size = New System.Drawing.Size(604, 57)
        Me._scriptsPanel3.TabIndex = 39
        '
        '_tspScriptSelectorLabel
        '
        Me._tspScriptSelectorLabel.BackColor = System.Drawing.Color.Transparent
        Me._tspScriptSelectorLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me._tspScriptSelectorLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me._tspScriptSelectorLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me._tspScriptSelectorLabel.Location = New System.Drawing.Point(3, 6)
        Me._tspScriptSelectorLabel.Name = "_tspScriptSelectorLabel"
        Me._tspScriptSelectorLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._tspScriptSelectorLabel.Size = New System.Drawing.Size(598, 16)
        Me._tspScriptSelectorLabel.TabIndex = 17
        Me._tspScriptSelectorLabel.Text = "Script File: "
        '
        '_scriptsPanel2
        '
        Me._scriptsPanel2.Controls.Add(Me._retainCodeOutlineToggle)
        Me._scriptsPanel2.Controls.Add(Me._scriptNameTextBoxLabel)
        Me._scriptsPanel2.Controls.Add(Me._scriptNameTextBox)
        Me._scriptsPanel2.Location = New System.Drawing.Point(3, 23)
        Me._scriptsPanel2.Name = "_scriptsPanel2"
        Me._scriptsPanel2.Size = New System.Drawing.Size(577, 59)
        Me._scriptsPanel2.TabIndex = 38
        '
        '_retainCodeOutlineToggle
        '
        Me._retainCodeOutlineToggle.AutoSize = True
        Me._retainCodeOutlineToggle.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._retainCodeOutlineToggle.Location = New System.Drawing.Point(320, 21)
        Me._retainCodeOutlineToggle.Name = "_retainCodeOutlineToggle"
        Me._retainCodeOutlineToggle.Size = New System.Drawing.Size(151, 21)
        Me._retainCodeOutlineToggle.TabIndex = 2
        Me._retainCodeOutlineToggle.Text = "Retain Code Outline"
        Me._retainCodeOutlineToggle.UseVisualStyleBackColor = True
        '
        '_scriptNameTextBoxLabel
        '
        Me._scriptNameTextBoxLabel.BackColor = System.Drawing.Color.Transparent
        Me._scriptNameTextBoxLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me._scriptNameTextBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me._scriptNameTextBoxLabel.Location = New System.Drawing.Point(4, 7)
        Me._scriptNameTextBoxLabel.Name = "_scriptNameTextBoxLabel"
        Me._scriptNameTextBoxLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._scriptNameTextBoxLabel.Size = New System.Drawing.Size(80, 16)
        Me._scriptNameTextBoxLabel.TabIndex = 0
        Me._scriptNameTextBoxLabel.Text = "Script Name: "
        '
        '_scriptsPanel1
        '
        Me._scriptsPanel1.Controls.Add(Me._loadScriptButton)
        Me._scriptsPanel1.Controls.Add(Me._loadAndRunButton)
        Me._scriptsPanel1.Controls.Add(Me._runScriptButton)
        Me._scriptsPanel1.Dock = System.Windows.Forms.DockStyle.Right
        Me._scriptsPanel1.Location = New System.Drawing.Point(610, 0)
        Me._scriptsPanel1.Name = "_scriptsPanel1"
        Me._scriptsPanel1.Size = New System.Drawing.Size(174, 474)
        Me._scriptsPanel1.TabIndex = 37
        '
        '_functionsTabPage
        '
        Me._functionsTabPage.Controls.Add(Me._functionsTableLayoutPanel)
        Me._functionsTabPage.Location = New System.Drawing.Point(4, 22)
        Me._functionsTabPage.Name = "_functionsTabPage"
        Me._functionsTabPage.Size = New System.Drawing.Size(784, 474)
        Me._functionsTabPage.TabIndex = 2
        Me._functionsTabPage.Text = "FUNCTIONS"
        Me._functionsTabPage.UseVisualStyleBackColor = True
        '
        '_functionsTableLayoutPanel
        '
        Me._functionsTableLayoutPanel.ColumnCount = 3
        Me._functionsTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7.0!))
        Me._functionsTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._functionsTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
        Me._functionsTableLayoutPanel.Controls.Add(Me._functionCodeTextBox, 1, 5)
        Me._functionsTableLayoutPanel.Controls.Add(Me._functionsPanel1, 1, 1)
        Me._functionsTableLayoutPanel.Controls.Add(Me._functionsTableLayoutPanel5, 1, 3)
        Me._functionsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._functionsTableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me._functionsTableLayoutPanel.Name = "_functionsTableLayoutPanel"
        Me._functionsTableLayoutPanel.RowCount = 7
        Me._functionsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me._functionsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._functionsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._functionsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._functionsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._functionsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me._functionsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me._functionsTableLayoutPanel.Size = New System.Drawing.Size(784, 474)
        Me._functionsTableLayoutPanel.TabIndex = 0
        '
        '_functionCodeTextBox
        '
        Me._functionCodeTextBox.AcceptsReturn = True
        Me._functionCodeTextBox.BackColor = System.Drawing.SystemColors.Window
        Me._functionCodeTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._functionCodeTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me._functionCodeTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._functionCodeTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me._functionCodeTextBox.Location = New System.Drawing.Point(10, 125)
        Me._functionCodeTextBox.MaxLength = 0
        Me._functionCodeTextBox.Multiline = True
        Me._functionCodeTextBox.Name = "_functionCodeTextBox"
        Me._functionCodeTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._functionCodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me._functionCodeTextBox.Size = New System.Drawing.Size(766, 337)
        Me._functionCodeTextBox.TabIndex = 1
        Me._functionCodeTextBox.Text = "function sum ( ... )" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  local result = 0" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  for index, value in ipairs( arg ) do " &
    "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    result = result + value" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  end" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  print ( table.concat( arg , "" + "" ) .. """ &
    " = "" .. result )" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "end" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        '_functionsPanel1
        '
        Me._functionsPanel1.Controls.Add(Me._functionArgsTextBox)
        Me._functionsPanel1.Controls.Add(Me._functionNameTextBox)
        Me._functionsPanel1.Controls.Add(Me._callFunctionButton)
        Me._functionsPanel1.Controls.Add(Me._functionNameTextBoxLabel)
        Me._functionsPanel1.Controls.Add(Me._functionArgsTextBoxLabel)
        Me._functionsPanel1.Location = New System.Drawing.Point(10, 13)
        Me._functionsPanel1.Name = "_functionsPanel1"
        Me._functionsPanel1.Size = New System.Drawing.Size(519, 69)
        Me._functionsPanel1.TabIndex = 0
        '
        '_functionArgsTextBox
        '
        Me._functionArgsTextBox.AcceptsReturn = True
        Me._functionArgsTextBox.BackColor = System.Drawing.SystemColors.Window
        Me._functionArgsTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._functionArgsTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._functionArgsTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me._functionArgsTextBox.Location = New System.Drawing.Point(100, 36)
        Me._functionArgsTextBox.MaxLength = 0
        Me._functionArgsTextBox.Name = "_functionArgsTextBox"
        Me._functionArgsTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._functionArgsTextBox.Size = New System.Drawing.Size(305, 25)
        Me._functionArgsTextBox.TabIndex = 3
        Me._functionArgsTextBox.Text = "1,2"
        '
        '_functionNameTextBox
        '
        Me._functionNameTextBox.AcceptsReturn = True
        Me._functionNameTextBox.BackColor = System.Drawing.SystemColors.Window
        Me._functionNameTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._functionNameTextBox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._functionNameTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me._functionNameTextBox.Location = New System.Drawing.Point(100, 8)
        Me._functionNameTextBox.MaxLength = 0
        Me._functionNameTextBox.Name = "_functionNameTextBox"
        Me._functionNameTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._functionNameTextBox.Size = New System.Drawing.Size(305, 25)
        Me._functionNameTextBox.TabIndex = 1
        Me._functionNameTextBox.Text = "sum"
        '
        '_functionNameTextBoxLabel
        '
        Me._functionNameTextBoxLabel.BackColor = System.Drawing.Color.Transparent
        Me._functionNameTextBoxLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me._functionNameTextBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me._functionNameTextBoxLabel.Location = New System.Drawing.Point(3, 8)
        Me._functionNameTextBoxLabel.Name = "_functionNameTextBoxLabel"
        Me._functionNameTextBoxLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._functionNameTextBoxLabel.Size = New System.Drawing.Size(105, 22)
        Me._functionNameTextBoxLabel.TabIndex = 0
        Me._functionNameTextBoxLabel.Text = "Function Name: "
        Me._functionNameTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_functionArgsTextBoxLabel
        '
        Me._functionArgsTextBoxLabel.BackColor = System.Drawing.Color.Transparent
        Me._functionArgsTextBoxLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me._functionArgsTextBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me._functionArgsTextBoxLabel.Location = New System.Drawing.Point(3, 36)
        Me._functionArgsTextBoxLabel.Name = "_functionArgsTextBoxLabel"
        Me._functionArgsTextBoxLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._functionArgsTextBoxLabel.Size = New System.Drawing.Size(105, 22)
        Me._functionArgsTextBoxLabel.TabIndex = 2
        Me._functionArgsTextBoxLabel.Text = "Arguments: "
        Me._functionArgsTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_functionsTableLayoutPanel5
        '
        Me._functionsTableLayoutPanel5.ColumnCount = 3
        Me._functionsTableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._functionsTableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._functionsTableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._functionsTableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me._functionsTableLayoutPanel5.Controls.Add(Me._loadFunctionButton, 2, 0)
        Me._functionsTableLayoutPanel5.Controls.Add(Me._functionCodeTextBoxLabel, 0, 0)
        Me._functionsTableLayoutPanel5.Controls.Add(Me._callInstructionsLabel, 1, 0)
        Me._functionsTableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me._functionsTableLayoutPanel5.Location = New System.Drawing.Point(10, 87)
        Me._functionsTableLayoutPanel5.Name = "_functionsTableLayoutPanel5"
        Me._functionsTableLayoutPanel5.RowCount = 1
        Me._functionsTableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._functionsTableLayoutPanel5.Size = New System.Drawing.Size(766, 33)
        Me._functionsTableLayoutPanel5.TabIndex = 25
        '
        '_functionCodeTextBoxLabel
        '
        Me._functionCodeTextBoxLabel.BackColor = System.Drawing.Color.Transparent
        Me._functionCodeTextBoxLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me._functionCodeTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._functionCodeTextBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me._functionCodeTextBoxLabel.Location = New System.Drawing.Point(3, 0)
        Me._functionCodeTextBoxLabel.Name = "_functionCodeTextBoxLabel"
        Me._functionCodeTextBoxLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._functionCodeTextBoxLabel.Size = New System.Drawing.Size(100, 33)
        Me._functionCodeTextBoxLabel.TabIndex = 0
        Me._functionCodeTextBoxLabel.Text = "Function Code: "
        Me._functionCodeTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        '_callInstructionsLabel
        '
        Me._callInstructionsLabel.BackColor = System.Drawing.Color.Transparent
        Me._callInstructionsLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me._callInstructionsLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me._callInstructionsLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me._callInstructionsLabel.Location = New System.Drawing.Point(109, 0)
        Me._callInstructionsLabel.Name = "_callInstructionsLabel"
        Me._callInstructionsLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._callInstructionsLabel.Size = New System.Drawing.Size(519, 33)
        Me._callInstructionsLabel.TabIndex = 30
        Me._callInstructionsLabel.Text = "Values returned by the instrument show under the Console tab. "
        Me._callInstructionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_instrumentTabPage
        '
        Me._instrumentTabPage.Controls.Add(Me._instrumentTableLayoutPanel)
        Me._instrumentTabPage.Location = New System.Drawing.Point(4, 22)
        Me._instrumentTabPage.Name = "_instrumentTabPage"
        Me._instrumentTabPage.Size = New System.Drawing.Size(784, 474)
        Me._instrumentTabPage.TabIndex = 3
        Me._instrumentTabPage.Text = "INSTRUMENT"
        Me._instrumentTabPage.UseVisualStyleBackColor = True
        '
        '_instrumentTableLayoutPanel
        '
        Me._instrumentTableLayoutPanel.ColumnCount = 7
        Me._instrumentTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._instrumentTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._instrumentTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._instrumentTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._instrumentTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._instrumentTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me._instrumentTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me._instrumentTableLayoutPanel.Controls.Add(Me._instrumentPanel3, 1, 0)
        Me._instrumentTableLayoutPanel.Controls.Add(Me._instrumentPanel2, 5, 0)
        Me._instrumentTableLayoutPanel.Controls.Add(Me._instrumentPanel1, 3, 0)
        Me._instrumentTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me._instrumentTableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me._instrumentTableLayoutPanel.Name = "_instrumentTableLayoutPanel"
        Me._instrumentTableLayoutPanel.RowCount = 1
        Me._instrumentTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._instrumentTableLayoutPanel.Size = New System.Drawing.Size(784, 123)
        Me._instrumentTableLayoutPanel.TabIndex = 53
        '
        '_instrumentPanel3
        '
        Me._instrumentPanel3.Controls.Add(Me._groupTriggerButton)
        Me._instrumentPanel3.Controls.Add(Me._deviceClearButton)
        Me._instrumentPanel3.Location = New System.Drawing.Point(59, 3)
        Me._instrumentPanel3.Name = "_instrumentPanel3"
        Me._instrumentPanel3.Size = New System.Drawing.Size(160, 113)
        Me._instrumentPanel3.TabIndex = 54
        '
        '_instrumentPanel2
        '
        Me._instrumentPanel2.Controls.Add(Me._showPromptsCheckBox)
        Me._instrumentPanel2.Controls.Add(Me._showErrorsCheckBox)
        Me._instrumentPanel2.Location = New System.Drawing.Point(531, 3)
        Me._instrumentPanel2.Name = "_instrumentPanel2"
        Me._instrumentPanel2.Size = New System.Drawing.Size(194, 113)
        Me._instrumentPanel2.TabIndex = 52
        '
        '_instrumentPanel1
        '
        Me._instrumentPanel1.Controls.Add(Me._abortButton)
        Me._instrumentPanel1.Controls.Add(Me._resetLocalNodeButton)
        Me._instrumentPanel1.Location = New System.Drawing.Point(281, 3)
        Me._instrumentPanel1.Name = "_instrumentPanel1"
        Me._instrumentPanel1.Size = New System.Drawing.Size(188, 113)
        Me._instrumentPanel1.TabIndex = 51
        '
        '_messagesTabPage
        '
        Me._messagesTabPage.Controls.Add(Me._TraceMessagesBox)
        Me._messagesTabPage.Location = New System.Drawing.Point(4, 22)
        Me._messagesTabPage.Name = "_messagesTabPage"
        Me._messagesTabPage.Size = New System.Drawing.Size(784, 474)
        Me._messagesTabPage.TabIndex = 4
        Me._MessagesTabPage.Text = "Log"
        Me._messagesTabPage.UseVisualStyleBackColor = True
        '
        '_TraceMessagesBox
        '
        Me._TraceMessagesBox.AlertLevel = System.Diagnostics.TraceEventType.Warning
        Me._TraceMessagesBox.BackColor = System.Drawing.SystemColors.Info
        Me._TraceMessagesBox.CausesValidation = False
        Me._TraceMessagesBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me._TraceMessagesBox.Font = New System.Drawing.Font("Consolas", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._TraceMessagesBox.Location = New System.Drawing.Point(0, 0)
        Me._TraceMessagesBox.Multiline = True
        Me._TraceMessagesBox.Name = "_TraceMessagesBox"
        Me._TraceMessagesBox.PresetCount = 50
        Me._TraceMessagesBox.ReadOnly = True
        Me._TraceMessagesBox.ResetCount = 100
        Me._TraceMessagesBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me._TraceMessagesBox.Size = New System.Drawing.Size(784, 474)
        Me._TraceMessagesBox.TabCaption = "Log"
        Me._TraceMessagesBox.TabIndex = 35
        '
        '_statusStrip
        '
        Me._statusStrip.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._statusLabel, Me._srqStatusLabel, Me._tspStatusLabel})
        Me._statusStrip.Location = New System.Drawing.Point(0, 541)
        Me._statusStrip.Name = "_statusStrip"
        Me._statusStrip.ShowItemToolTips = True
        Me._statusStrip.Size = New System.Drawing.Size(792, 25)
        Me._statusStrip.TabIndex = 0
        Me._statusStrip.Text = "Ready"
        '
        '_statusLabel
        '
        Me._statusLabel.AutoSize = False
        Me._statusLabel.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._statusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._statusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._statusLabel.Margin = New System.Windows.Forms.Padding(0)
        Me._statusLabel.Name = "_statusLabel"
        Me._statusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me._statusLabel.Size = New System.Drawing.Size(630, 25)
        Me._statusLabel.Spring = True
        Me._statusLabel.Text = "<operation status>"
        Me._statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._statusLabel.ToolTipText = "Operation Status"
        '
        '_srqStatusLabel
        '
        Me._srqStatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._srqStatusLabel.Name = "_srqStatusLabel"
        Me._srqStatusLabel.Padding = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me._srqStatusLabel.Size = New System.Drawing.Size(38, 20)
        Me._srqStatusLabel.Text = "0x00"
        Me._srqStatusLabel.ToolTipText = "SRQ Value"
        '
        '_tspStatusLabel
        '
        Me._tspStatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me._tspStatusLabel.Name = "_tspStatusLabel"
        Me._tspStatusLabel.Padding = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me._tspStatusLabel.Size = New System.Drawing.Size(109, 20)
        Me._tspStatusLabel.Text = "CONTINUATION"
        Me._tspStatusLabel.ToolTipText = "TSP STATE"
        '
        'TestPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(792, 566)
        Me.Controls.Add(Me._tabControl)
        Me.Controls.Add(Me._controlsTableLayoutPanel)
        Me.Controls.Add(Me._statusStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(230, 203)
        Me.MaximizeBox = False
        Me.Name = "TestPanel"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me._controlsTableLayoutPanel.ResumeLayout(False)
        Me._controlsTableLayoutPanel1.ResumeLayout(False)
        CType(Me._errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabControl.ResumeLayout(False)
        Me._consoleTabPage.ResumeLayout(False)
        Me._consoleTableLayoutPanel.ResumeLayout(False)
        Me._inputPanel.ResumeLayout(False)
        Me._inputPanel.PerformLayout()
        Me._timingTableLayoutPanel.ResumeLayout(False)
        Me._timingTableLayoutPanel.PerformLayout()
        Me._outputPanel.ResumeLayout(False)
        Me._outputPanel.PerformLayout()
        Me._scriptsTabPage.ResumeLayout(False)
        Me._scriptsTableLayoutPanel.ResumeLayout(False)
        Me._scriptsPanel4.ResumeLayout(False)
        Me._scriptsPanel3.ResumeLayout(False)
        Me._scriptsPanel2.ResumeLayout(False)
        Me._scriptsPanel2.PerformLayout()
        Me._scriptsPanel1.ResumeLayout(False)
        Me._functionsTabPage.ResumeLayout(False)
        Me._functionsTableLayoutPanel.ResumeLayout(False)
        Me._functionsTableLayoutPanel.PerformLayout()
        Me._functionsPanel1.ResumeLayout(False)
        Me._functionsPanel1.PerformLayout()
        Me._functionsTableLayoutPanel5.ResumeLayout(False)
        Me._instrumentTabPage.ResumeLayout(False)
        Me._instrumentTableLayoutPanel.ResumeLayout(False)
        Me._instrumentPanel3.ResumeLayout(False)
        Me._instrumentPanel2.ResumeLayout(False)
        Me._instrumentPanel1.ResumeLayout(False)
        Me._messagesTabPage.ResumeLayout(False)
        Me._messagesTabPage.PerformLayout()
        Me._statusStrip.ResumeLayout(False)
        Me._statusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _SrqStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _TspStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents _ClearInterfaceButton As System.Windows.Forms.Button
    Private WithEvents _OutputPanel As System.Windows.Forms.Panel
    Private WithEvents _TimingTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _InputPanel As System.Windows.Forms.Panel
    Private WithEvents _ConsoleTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _InstrumentPanel1 As System.Windows.Forms.Panel
    Private WithEvents _InstrumentTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _InstrumentPanel2 As System.Windows.Forms.Panel
    Private WithEvents _ControlsTableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _ControlsTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _ScriptsPanel3 As System.Windows.Forms.Panel
    Private WithEvents _ScriptsPanel2 As System.Windows.Forms.Panel
    Private WithEvents _ScriptsPanel1 As System.Windows.Forms.Panel
    Private WithEvents _ScriptsPanel4 As System.Windows.Forms.Panel
    Private WithEvents _ScriptsTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _FunctionsPanel1 As System.Windows.Forms.Panel
    Private WithEvents _FunctionsTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _FunctionsTableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
    Private WithEvents _InstrumentPanel3 As System.Windows.Forms.Panel
    Private WithEvents _RefreshUserScriptsListButton As System.Windows.Forms.Button
    Private WithEvents _RetainCodeOutlineToggle As System.Windows.Forms.CheckBox
    Private WithEvents _ResourceSelectorConnector As isr.VI.Instrument.ResourceSelectorConnector

End Class