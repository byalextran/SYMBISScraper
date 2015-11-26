require "mechanize"

class SymbisScraper
  LOG_IN_PAGE = "https://my.symbisassessment.com/facilitators/sign_in"
  EMAIL = ""
  PASSWORD = ""

  def initialize
    @agent = Mechanize.new
    @agent.user_agent_alias = 'Mac FireFox'

    log_in
  end

  def download_sessions
    links = @page.links_with(:href => /\/session\/\d{1,2}/).each do |link|
      session = link.click
      filename = session.search("h1").text.gsub(/\W/, "")

      iframe = session.iframes.first.click

      m = iframe.body.match /"mobile":.*?"url":"(?<url>.*?)"/i

      `wget -O "#{filename}".mp4 "#{m['url']}"`
    end
  end

  private

  def log_in
    @page = @agent.get(LOG_IN_PAGE)

    form = @page.form_with(:id => 'new_facilitator')
    form["facilitator[email]"] = EMAIL
    form["facilitator[password]"] = PASSWORD

    @page = @agent.submit(form)
  end
end

ss = SymbisScraper.new
ss.download_sessions