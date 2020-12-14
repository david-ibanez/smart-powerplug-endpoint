namespace SmartPowerplugEndpoint.Models

open MongoDB.Bson

type Error =
    { Name: string
      Desc: string}

type Errors =
    { Errors: Error[] }

[<CLIMutable>]
type DeviceAuth =
    { Id: string
      User: string
      Pass: string
      Salt: string
      Algorimth: int }

[<CLIMutable>]
type Device =
    { Id: string
      FirmwareBuild: int
      LastLogin: string }

[<CLIMutable>]
type RawMeasure =
    { Id: string
      Measures: int64 array }

[<CLIMutable>]
type Led =
    { Id: int
      On: bool }
