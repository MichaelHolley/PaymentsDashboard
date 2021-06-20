import { Component, OnInit, ViewChild } from '@angular/core';
import { PaymentService } from '../../assets/shared/services/payment.service';
import { TagService } from '../../assets/shared/services/tag.service';
import { ChartComponent, ApexAxisChartSeries, ApexChart, ApexXAxis, ApexYAxis, ApexTitleSubtitle, ApexDataLabels, ApexPlotOptions, ApexFill, ApexTooltip } from "ng-apexcharts";
import { Payment, Tag } from '../../assets/shared/models/models';
import { ChartsService } from '../../assets/shared/services/charts.service';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  title: ApexTitleSubtitle;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  tooltip: ApexTooltip;
  labels: any;
  colors: any[];
};

@Component({
  selector: 'app-charts',
  templateUrl: './charts.component.html'
})
export class ChartsComponent implements OnInit {
  public scatterChartOptions: Partial<ChartOptions>;
  public monthlyBarChartOptions: Partial<ChartOptions>;
  public monthlyAveragePieChartOptions: Partial<ChartOptions>;

  constructor(
    private paymentsService: PaymentService,
    private tagService: TagService,
    private chartsService: ChartsService) { }

  ngOnInit() {
    let titleFontStyle = {
      fontFamily: 'Lato',
      fontSize: '16px'
    };

    // Scatter-Chart
    {
      this.scatterChartOptions = {
        title: {
          text: 'Scattered Payments',
          style: titleFontStyle
        },
        chart: {
          type: 'scatter',
          zoom: {
            type: 'xy'
          },
          toolbar: {
            show: false
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

      this.tagService.getPrimaryTags().subscribe(result => {
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
      this.monthlyBarChartOptions = {
        series: [],
        title: {
          text: 'Sum of Payments by Month',
          style: titleFontStyle
        },
        chart: {
          type: 'bar',
          stacked: true,
          toolbar: {
            show: false
          }
        },
        yaxis: {
          labels: {
            formatter: this.valueFormatter
          }
        },
        xaxis: {
          categories: []
        },
        plotOptions: {
          bar: {
            horizontal: false
          }
        },
        dataLabels: {
          enabled: false
        }
      };

      this.chartsService.getStackedBarChartByMonths().subscribe(result => {
        let values = [];
        let tags: Tag[] = [];
        result.forEach(mv => {
          this.monthlyBarChartOptions.xaxis.categories.push(new Date(mv.month).toLocaleString('en-EN', { month: 'short', year: 'numeric' }));
          mv.tagSums.forEach((ts, index) => {
            if (!values[index]) {
              values.push([]);
              tags.push(ts.tag);
            }

            values[index].push(ts.sum);
          });
        });

        for (let i = 0; i < values.length; i++) {
          this.monthlyBarChartOptions.series.push({
            name: tags[i].title,
            color: tags[i].hexColorCode,
            data: values[i],
          });
        }
      });
    }

    // Monthly Average Pie-Chart
    {
      this.monthlyAveragePieChartOptions = {
        series: [],
        chart: {
          height: 350,
          type: 'donut'
        },
        title: {
          text: 'Monthly Average',
          style: titleFontStyle
        },
        labels: [],
        colors: [],
        plotOptions: {
          pie: {
            donut: {
              labels: {
                show: true,
                name: {
                  show: true
                },
                value: {
                  show: true,
                  formatter: this.valueFormatter
                },
                total: {
                  show: true,
                  color: '#000000',
                  formatter: function (w) {
                    let sum = w.globals.seriesTotals.reduce((a, b) => { return a + b }, 0);
                    return Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(sum);
                  }
                }
              }
            }
          }
        },
        tooltip: {
          y: {
            formatter: this.valueFormatter
          }
        }
      };

      this.chartsService.getMonthlyAverageByTag().subscribe(result => {
        result.forEach(val => {
          this.monthlyAveragePieChartOptions.series.push(val.sum);
          this.monthlyAveragePieChartOptions.labels.push(val.tag.title);
          this.monthlyAveragePieChartOptions.colors.push(val.tag.hexColorCode);
        });
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
    return new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(val);
  }
}
