import { Component, OnInit, ViewChild } from '@angular/core';
import { PaymentService } from '../../assets/shared/services/payment.service';
import { TagService } from '../../assets/shared/services/tag.service';
import { ChartComponent, ApexAxisChartSeries, ApexChart, ApexXAxis, ApexYAxis, ApexDataLabels, ApexGrid, ApexTitleSubtitle } from "ng-apexcharts";

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  title: ApexTitleSubtitle;
  dataLabels: ApexDataLabels;
  grid: ApexGrid;
};

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html'
})
export class StatisticsComponent implements OnInit {
  public chartOptions: Partial<ChartOptions>;

  constructor(
    private paymentsService: PaymentService,
    private tagService: TagService) { }

  ngOnInit() {
    this.chartOptions = {
      title: { text: 'Payments by Tag' },
      chart: {
        type: 'scatter',
        zoom: {
          type: 'xy'
        }
      },
      series: [],
      xaxis: {
        type: 'datetime'
      },
      yaxis: {
        labels: {
          formatter: function (val) {
            return val.toFixed(2);
          }
        }
      }
    };


    this.tagService.getAllTags().subscribe(result => {
      result.forEach(tag => {
        let paymentValues = [];
        tag.payments.forEach(p => paymentValues.push([new Date(p.date).getTime(), p.amount]));
        console.log(paymentValues)
        this.chartOptions.series.push(
          {
            name: tag.title,
            color: tag.hexColorCode,
            data: paymentValues
          }
        );
      });
    });
  }
}
