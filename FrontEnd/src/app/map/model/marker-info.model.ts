import { GeoLocation } from "./geolocation";

export class MarkerInfo {
    iconUrl: string;
    title: string;
    label: string;
    location: GeoLocation;
    link: string;

    constructor(location: GeoLocation, icon: string, title:string, label:string, link: string){
        this.iconUrl = icon;
        this.title = title;
        this.label = label;
        this.location = location;
        this.link = link;
    }
} 