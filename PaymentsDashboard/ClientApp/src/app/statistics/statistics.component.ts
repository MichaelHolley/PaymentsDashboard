import { Component, OnInit, ViewChild } from '@angular/core';
import { PaymentService } from '../../assets/shared/services/payment.service';
import { TagService } from '../../assets/shared/services/tag.service';
import { ChartComponent, ApexAxisChartSeries, ApexChart, ApexXAxis, ApexYAxis, ApexTitleSubtitle, ApexDataLabels, ApexPlotOptions } from "ng-apexcharts";
import { Payment, Tag } from '../../assets/shared/models/models';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  title: ApexTitleSubtitle;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
};

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html'
})
export class StatisticsComponent implements OnInit {
  public scatterChartOptions: Partial<ChartOptions>;
  public dailySumChartOptions: Partial<ChartOptions>;

  constructor(
    private paymentsService: PaymentService,
    private tagService: TagService) { }

  ngOnInit() {
    // Scatter-Chart
    {
      this.scatterChartOptions = {
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
            formatter: this.valueFormatter
          }
        }
      };

      this.tagService.getAllTags().subscribe(result => {
        result.forEach(tag => {
          let values = [];
          tag.payments.forEach(p => values.push([new Date(p.date).getTime(), p.amount]));
          this.scatterChartOptions.series.push(
            {
              name: tag.title,
              color: tag.hexColorCode,
              data: values
            }
          );
        });
      });
    }

    // Stacked Bar-Chart
    {
      this.dailySumChartOptions = {
        title: { text: 'Sum of Payments by Date' },
        chart: {
          type: 'bar'
        },
        series: [],
        yaxis: {
          labels: {
            formatter: this.valueFormatter
          }
        },
        dataLabels: {
          enabled: false,
          formatter: this.valueFormatter
        }
      };

      this.paymentsService.getAllPayments().subscribe(payments => {
        let paymentsByDate: {
          date: Date,
          sum: number
        }[] = [];

        let startDate = new Date(payments[0].date);
        let lastDate = new Date(payments[payments.length - 1].date);

        while (startDate <= lastDate) {
          let tempSum = this.getSum(payments.filter(p => new Date(p.date).getTime() === startDate.getTime()));
          paymentsByDate.push({ date: new Date(startDate), sum: tempSum });
          startDate.setDate(startDate.getDate() + 1);
        }

        let dates = [];
        let values = [];
        paymentsByDate.forEach(pbd => {
          dates.push(pbd.date);
          values.push(pbd.sum);
        });

        this.dailySumChartOptions.series.push({
          name: 'Payments',
          color: '#FF6B35',
          data: values
        });

        this.dailySumChartOptions.xaxis = {
          type: 'datetime',
          categories: dates
        };
      });
    }
  }


  getSum(payments: Payment[]): number {
    let sum = 0;
    payments.forEach(p => {
      sum += p.amount;
    });

    return sum;
  }

  valueFormatter(val) {
    return val.toFixed(2);
  }
}
