import type { IMap, IOtherSource } from 'src/Types';

export enum RegionType {
    IgrekAmzp,
    MapyAmzp,
    Unknown,
}
export interface IRegion {
    id: string;
    regionName1: string;
    regionName2?: string;
    regionName3?: string;
    nwLat: number;
    nwLong: number;
    neLat: number;
    neLong: number;
    seLat: number;
    seLong: number;
    swLat: number;
    swLong: number;
    regionIdentity: string;
    type: RegionType;
    maps?: IMap[];
    otherSources?: IOtherSource[];
}