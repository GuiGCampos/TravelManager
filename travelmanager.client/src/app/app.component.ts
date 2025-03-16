import { Component } from '@angular/core';
import { RouteService } from './services/route.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  from = '';
  to = '';
  price!: number;
  routeId!: number;
  result: any;

  constructor(private routeService: RouteService) { }

  createRoute() {
    this.routeService.createRoute(this.from, this.to, this.price).subscribe(() => {
      alert('Rota criada com sucesso!');
    }, error =>
    {
      const erro = JSON.parse(error.error);
      alert(erro.error);
    });
  }

  deleteRoute() {
    this.routeService.deleteRoute(this.from, this.to).subscribe(() => {
      alert('Rota deletada com sucesso!');
    }, error => {
      const erro = JSON.parse(error.error);
      alert(erro.error);
    });
  }

  updateRoute() {
    this.routeService.updateRoute(this.routeId, this.from, this.to, this.price).subscribe(() => {
      alert('Rota atualizada com sucesso!');
    }, error => {
      const erro = JSON.parse(error.error);
      alert(erro.error);
    });
  }

  getRoute() {
    this.routeService.getRoute(this.from, this.to).subscribe(response => {
      this.result = response;
    }, error => {
      const erro = JSON.parse(error.error);
      alert(erro.error);
    });
  }

  getLowestPrice() {
    this.routeService.getLowestPrice(this.from, this.to).subscribe(response => {
      this.result = response;
    }, error => {
      const erro = JSON.parse(error.error);
      alert(erro.error);
    });
  }
}
