import { GeoLocation } from "./geolocation";

export class Polyline {
    public path: GeoLocation[]
    public color: string
    public icon: any

    constructor(path: GeoLocation[], color: string, icon: any){
        this.color = color;
        this.path = path;
        this.icon = icon
    }

    addLocation(location: GeoLocation){
        this.path.push(location)
    }
}