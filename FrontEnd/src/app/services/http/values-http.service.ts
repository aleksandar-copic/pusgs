import {BaseHttpService} from './base-http.service';

export class ValuesHttpService extends BaseHttpService<string>{
    specificUrl = "/api/values"
}

