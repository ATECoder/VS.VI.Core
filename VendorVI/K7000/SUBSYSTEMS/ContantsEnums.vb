#Region " TYPES "

    ''' <summary>Enumerates the status byte flags of the operation event register.</summary>
    <System.Flags()>
    Public Enum OperationEvents
        <ComponentModel.Description("None")> None = 0
        <ComponentModel.Description("Calibrating")> Calibrating = 1
        <ComponentModel.Description("Settling")> Settling = 2
        <ComponentModel.Description("Sweeping")> Sweeping = 8
        <ComponentModel.Description("Waiting For Trigger")> WaitingForTrigger = 32
        <ComponentModel.Description("Waiting For Arm")> WaitingForArm = 64
        <ComponentModel.Description("Idle")> Idle = 1024
    End Enum

    ''' <summary>Enumerates the status byte flags of the operation transition event register.</summary>
    <System.Flags()>
    Public Enum OperationTransitionEvents
        <ComponentModel.Description("None")> None = 0
        <ComponentModel.Description("Settling")> Settling = 2
        <ComponentModel.Description("Waiting For Trigger")> WaitingForTrigger = 32
        <ComponentModel.Description("Waiting For Arm")> WaitingForArm = 64
        <ComponentModel.Description("Idle ")> Idle = 1024
    End Enum

    ''' <summary>Enumerates the status byte flags of the questionable event register.</summary>
    <System.Flags()>
    Public Enum QuestionableEvents
        <ComponentModel.Description("None")> None = 0
        <ComponentModel.Description("Calibration Event")> CalibrationEvent = 256
        <ComponentModel.Description("Command Warning")> CommandWarning = 16384
        'All = 32767
    End Enum

#End Region
