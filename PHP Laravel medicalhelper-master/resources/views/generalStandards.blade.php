@extends('layouts.app')

@section('content')
<div class="container">
    <div class="accordion">
        @isset($response)
        @foreach($response as $standard)
        <div class="mb-5 ">
            <div class="fs-4 standard-name d-flex" data-bs-toggle="collapse" data-bs-target="#standart_{{md5($standard['name'])}}">
                <div>{{ $standard['name'] }}</div>
                <div class="fs-4 fw-bold ms-3">{{ $standard['mkbGroup'] }}</div>
            </div>

            <div id="standart_{{md5($standard['name'])}}" class="accordion-collapse collapse">
                <div class="fs-4">{{ $standard['purpose'] }}</div>
                <div>{{ $standard['description'] }}</div>
                <div>
                    <table class="table">
                        @foreach($standard['sections'] as $section)
                        <tr>
                            <td colspan="4" class="fs-4">{{ $section['sectionName'] }}</td>
                        </tr>
                        @foreach($section['servicesGroups'] as $group)
                        <tr>
                            <td colspan="4" class="fs-5 fw-bold">{{ $group['groupName'] }}</td>
                        </tr>
                        <tr>
                            <th>Код медицинской услуги</th>
                            <th>Наименование медицинской услуги</th>
                            <th>Усредненный показатель частоты предоставления</th>
                            <th>Усредненный показатель кратности применения</th>
                        </tr>
                        @foreach($group['services'] as $service)
                        <tr>
                            <td>{{ $service['code'] }}</td>
                            <td>{{ $service['name'] }}</td>
                            <td>{{ $service['applicationFrequencyIndex'] }}</td>
                            <td>{{ $service['rateOfSubmitting'] }}</td>
                        </tr>

                        @endforeach
                        @endforeach
                        @endforeach
                    </table>
                </div>
            </div>

        </div>
        @endforeach
        @endisset
    </div>
</div>
@endsection