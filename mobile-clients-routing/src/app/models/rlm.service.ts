import { Injectable } from '@angular/core';
//import { Connection, hubConnection } from 'signalr';
//import { Http } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { element } from 'protractor';
import { Race } from './race';
import { Tournament } from './tournament';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RlmService {

  //private connection: Connection;
  private selectedTournamentState = new BehaviorSubject<Tournament>(null);
  public selectedTournament: Observable<Tournament>;

  static readonly SERVICE_READY: string = 'serviceReady';
  private apiRoot = '';

  constructor(public http: HttpClient) { 
    // this.connection = hubConnection();
    // this.connection.start()
    // .done((function () { 
    //     console.log("Now connected to RLM service!!!: ");
    //     //this.fire(RlmService.SERVICE_READY);
    // }).bind(this))
    // .fail(function () { console.log("Unable to connect to service"); });
    this.selectedTournament = this.selectedTournamentState.asObservable();

    if (!environment.production) {
      this.apiRoot = 'http://localhost';
    }
  }

  public GetLineup() {
    return 'Foo'; //this.http.get('http://localhost/api/lineup/1').pipe(map(res => res.json()));
  }

  public GetTournaments() {
    return this.http.get(`${this.apiRoot}/api/tournament`);
  }

  public GetTournament(tournamentId: number) {
    return this.http.get(`${this.apiRoot}/api/tournament/${tournamentId}`);
  }

  public GetRaces(tournamentId: number) {
    //let result: Array<Race> = new Array<Race>();

    return this.http.get(`${this.apiRoot}/api/tournament/${tournamentId}/race`);

    //return result;
  }

  public SetSelectedTournament(tournamentId: number) {
    this.GetTournament(tournamentId).subscribe((tournament: Tournament) => {
      this.selectedTournamentState.next(tournament);
    });
  }
}
